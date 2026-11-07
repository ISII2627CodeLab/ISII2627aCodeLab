using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.Impresora3DDTOs;

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
            var reserva = await _context.ReservaImpresoras
                .Include(r => r.LineaReservas)
                    .ThenInclude(lr => lr.Impresora3D)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                _logger.LogWarning($"{DateTime.Now} Reserva con ID {id} no encontrada.");
                return NotFound();
            }

            var reservaDTO = new ReservaDetailDTO(
                reserva.Id,
                reserva.FechaReserva,
                reserva.NombreCliente,
                reserva.ApellidosCliente,
                reserva.DireccionFacturacion,
                reserva.PrecioTotal,
                reserva.MetodoPago,
                reserva.LineaReservas.Select(le => new LineaReservaDetailsDTO(
                    le.Impresora3D != null ? le.Impresora3D.Nombre : string.Empty,
                    le.TiempoReserva,
                    le.PrecioSubtotal
                )).ToList()
            );

            return Ok(reservaDTO);
        }
    }
}