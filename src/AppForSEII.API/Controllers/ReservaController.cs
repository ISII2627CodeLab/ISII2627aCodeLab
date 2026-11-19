using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(ApplicationDbContext context, ILogger<ReservaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReservaDetailDTO>> GetReservaDetails(int id)
        {
            var reserva = await _context.ReservaImpresoras
                .Include(r => r.LineaReservas)
                    .ThenInclude(lr => lr.Impresora3D)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                _logger.LogWarning($"Reserva con ID {id} no encontrada.");
                return NotFound();
            }

            var lineasDTO = reserva.LineaReservas.Select(le => new LineaReservaDTO(
                le.Impresora3DId,
                le.Impresora3D != null ? le.Impresora3D.Nombre : string.Empty,
                le.Impresora3D != null ? le.Impresora3D.Modelo : string.Empty,
                le.Impresora3D != null ? le.Impresora3D.Tipo.ToString() : string.Empty,
                le.Impresora3D != null ? le.Impresora3D.Descripcion : string.Empty,
                le.TiempoReserva,
                le.Impresora3D != null ? le.Impresora3D.PrecioKilovatioHora : 0,
                le.Impresora3D != null ? le.Impresora3D.PrecioReserva : 0,
                le.PrecioSubtotal
            )).ToList();

            var reservaDetailDTO = new ReservaDetailDTO(
                reserva.Id,
                reserva.FechaReserva,
                reserva.NombreCliente,
                reserva.ApellidosCliente,
                reserva.DireccionFacturacion,
                reserva.PrecioTotal,
                reserva.MetodoPago,
                lineasDTO
            );

            return Ok(reservaDetailDTO);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ReservaDetailDTO>> CreateReserva([FromBody] ReservaDTO reservaDTO)
        {
            // Comprobar si el cliente existe
            var cliente = await _context.Users.OfType<Cliente>()
                .FirstOrDefaultAsync(c => c.Name == reservaDTO.NombreCliente && c.Surname == reservaDTO.ApellidosCliente);

            if (cliente == null)
            {
                _logger.LogError($"{DateTime.Now} Exception: El cliente no existe.");
                return BadRequest("El cliente no existe.");
            }

            // Flujo alternativo: Lista vacía de líneas de reserva
            if (reservaDTO.LineasReserva == null || !reservaDTO.LineasReserva.Any())
            {
                _logger.LogError($"{DateTime.Now} Exception: No se han incluido líneas de reserva.");
                return BadRequest("La reserva debe contener al menos una impresora.");
            }

            //ejercicio2
            if (string.IsNullOrEmpty(reservaDTO.NombreCliente) || reservaDTO.NombreCliente.Length < 3)
            {
                _logger.LogError($"{DateTime.Now} Exception: El nombre tiene menos de 3 caracteres.");
                return BadRequest("¡Error! El nombre debe tener al menos 3 caracteres");
            }
 
            decimal precioTotal = 0;
            var lineasReservaEntidad = new List<LineaReserva>();

            foreach (var lineaDto in reservaDTO.LineasReserva)
            {
                var impresora = await _context.Impresoras3Ds.FindAsync(lineaDto.Impresora3DId);

                // Validar si la impresora existe
                if (impresora == null)
                {
                    _logger.LogError($"{DateTime.Now} Exception: Impresora con ID {lineaDto.Impresora3DId} no encontrada.");
                    return BadRequest($"La impresora con ID {lineaDto.Impresora3DId} no existe.");
                }

                // Cálculo del subtotal por línea
                decimal factorTiempo = (int)lineaDto.TiempoReserva + 1;
                decimal precioSubtotal = impresora.PrecioReserva * factorTiempo;
                precioTotal += precioSubtotal;

                var lineaReserva = new LineaReserva(lineaDto.TiempoReserva, precioSubtotal, impresora.Id);
                lineasReservaEntidad.Add(lineaReserva);
            }

            // Crear la entidad ReservaImpresora
            var reserva = new ReservaImpresora(
                reservaDTO.NombreCliente,
                reservaDTO.ApellidosCliente,
                reservaDTO.DireccionFacturacion,
                precioTotal,
                reservaDTO.MetodoPago
            );

            reserva.LineaReservas = lineasReservaEntidad;

            // Guardar en la base de datos
            _context.ReservaImpresoras.Add(reserva);
            await _context.SaveChangesAsync();

            // Recargar con relaciones para el DTO de detalle
            var reservaCreada = await _context.ReservaImpresoras
                .Include(r => r.LineaReservas)
                    .ThenInclude(lr => lr.Impresora3D)
                .Where(r => r.Id == reserva.Id)
                .FirstAsync();

            var detalleDTO = new ReservaDetailDTO(
                reservaCreada.Id,
                reservaCreada.FechaReserva,
                reservaCreada.NombreCliente,
                reservaCreada.ApellidosCliente,
                reservaCreada.DireccionFacturacion,
                reservaCreada.PrecioTotal,
                reservaCreada.MetodoPago,
                reservaCreada.LineaReservas.Select(le => new LineaReservaDTO(
                    le.Impresora3DId,
                    le.Impresora3D!.Nombre,
                    le.Impresora3D!.Modelo,
                    le.Impresora3D!.Tipo.ToString(),
                    le.Impresora3D!.Descripcion,
                    le.TiempoReserva,
                    le.Impresora3D!.PrecioKilovatioHora,
                    le.Impresora3D!.PrecioReserva,
                    le.PrecioSubtotal
                )).ToList()
            );

            return CreatedAtAction("GetReservaDetails", "Reserva", new { id = reserva.Id }, detalleDTO);
        }
    }
}