using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraModelo3DDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraModelo3DDetailsController> _logger;

        public CompraModelo3DDetailsController(ApplicationDbContext context, ILogger<CompraModelo3DDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //ver los detalles de una compra de modelos 3D
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompraModelo3DDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompraModelo3D(int id)
        {
            // buscar la compra con sus líneas, el modelo de cada línea y su licencia
            var compra = await _context.ComprasModelo3D
                .Include(c => c.LineasCompra)
                    .ThenInclude(l => l.Modelo)
                        .ThenInclude(m => m.Licencia)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (compra == null)
            {
                _logger.LogError($"{DateTime.Now} Error: la compra con id {id} no existe.");
                return NotFound();
            }

            var compraDetailDTO = new CompraModelo3DDetailDTO(
                compra.Id,
                compra.FechaCompra,
                compra.NombreCliente,
                compra.ApellidosCliente,
                compra.CorreoElectronico,
                compra.DireccionFacturacion,
                compra.MetodoPago,
                compra.Descripcion,
                compra.PrecioTotal,
                compra.LineasCompra.Select(l => new LineaCompraModelo3DDetailDTO(
                    l.Modelo.Nombre,
                    l.Modelo.Categoria,
                    l.Modelo.Formato,
                    l.PrecioUnidad,
                    l.CantidadLicencias,
                    l.Subtotal,
                    l.Modelo.Licencia.FechaExpiracion
                )).ToList()
            );

            return Ok(compraDetailDTO);
        }
    }
}