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
    public class ReservaDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservaDetailController> _logger;

        public ReservaDetailController(ApplicationDbContext context, ILogger<ReservaDetailController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ActionName("GetReservaDetails")]
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
                _logger.LogError($"{DateTime.Now} Exception: Reserva con ID {id} no encontrada.");
                return NotFound();
            }

            var detalleDTO = new ReservaDetailDTO(
                reserva.Id,
                reserva.FechaReserva,
                reserva.NombreCliente,
                reserva.ApellidosCliente,
                reserva.DireccionFacturacion,
                reserva.PrecioTotal,
                reserva.MetodoPago,
                reserva.LineaReservas.Select(le => new LineaReservaDetailsDTO(
                    le.Impresora3D != null ? le.Impresora3D.Nombre : string.Empty,
                    le.Impresora3D != null ? le.Impresora3D.Modelo : string.Empty,
                    le.Impresora3D != null ? le.Impresora3D.Tipo.ToString() : string.Empty, // Convertido a string con .ToString()
                    le.Impresora3D != null ? le.Impresora3D.Descripcion : string.Empty,
                    le.TiempoReserva,
                    le.Impresora3D != null ? le.Impresora3D.PrecioKilovatioHora : 0,
                    le.Impresora3D != null ? le.Impresora3D.PrecioReserva : 0,
                    le.PrecioSubtotal
                )).ToList() 
            );
 
            return Ok(detalleDTO);
        }
    }
}