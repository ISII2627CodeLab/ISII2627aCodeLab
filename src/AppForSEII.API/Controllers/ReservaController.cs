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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ReservaDetailDTO>> CreateReserva([FromBody] ReservaDTO reservaDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{DateTime.Now} Exception: ModelState is not valid when creating a reserva");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Flujo alternativo: Lista vacía de líneas de reserva
            if (reservaDTO.LineasReserva == null || !reservaDTO.LineasReserva.Any())
            {
                _logger.LogError($"{DateTime.Now} Exception: No se han incluido líneas de reserva.");
                return BadRequest("La reserva debe contener al menos una impresora.");
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
                reservaCreada.LineaReservas.Select(le => new LineaReservaDetailsDTO(
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