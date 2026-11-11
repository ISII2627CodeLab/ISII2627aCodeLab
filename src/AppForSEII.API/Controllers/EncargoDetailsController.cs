using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncargoDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncargoDetailsController> _logger;

        public EncargoDetailsController(ApplicationDbContext context, ILogger<EncargoDetailsController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("EncargoDetailsController initialized");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EncargoDetailsDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<EncargoDetailsDTO>> GetEncargo(int id)
        {
            var encargo = await _context.EncargosImpresion
                .Include(e => e.LineasEncargo)
                    .ThenInclude(le => le.Pieza)
                .Include(e => e.LineasEncargo)
                    .ThenInclude(le => le.MaterialSeleccionado)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (encargo == null)
            {
                _logger.LogWarning($"{DateTime.Now} Encargo con ID {id} no encontrado.");
                return NotFound();
            }

            var encargoDTO = new EncargoDetailsDTO(
                encargo.Id,
                encargo.FechaEncargo,
                encargo.NombreCliente,
                encargo.ApellidosCliente,
                encargo.DireccionEnvio,
                encargo.NumeroTelefono,
                encargo.MetodoPago,
                encargo.Descripcion,
                encargo.PrecioTotal,
                encargo.LineasEncargo.Select(le => new LineaEncargoDetailsDTO(
                    le.Pieza.Nombre,
                    le.MaterialSeleccionado.Nombre,
                    le.Cantidad,
                    le.PrecioUnidad,
                    le.Subtotal,
                    encargo.Descripcion
                )).ToList()
            );

            return Ok(encargoDTO);
        }
    }
}