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
    public class ReservaDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservaDetailsController> _logger;

        public ReservaDetailsController(ApplicationDbContext context, ILogger<ReservaDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReservaDetailDTO>> GetReservaDetails(int id)
        {
            // Buscamos la reserva incluyendo sus líneas y la impresora asociada
            var reserva = await _context.ReservaImpresoras
                .Include(r => r.LineaReservas)
                    .ThenInclude(lr => lr.Impresora3D)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            // Caso de error: Si no existe, devuelve NotFound (cubre el segundo test)
            if (reserva == null)
            {
                _logger.LogWarning($"Reserva con ID {id} no encontrada.");
                return NotFound();
            }

            // Mapeo a DTO de detalles
            var lineasDTO = reserva.LineaReservas.Select(le => new LineaReservaDetailsDTO(
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
    }
}