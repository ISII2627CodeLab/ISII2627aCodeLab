using AppForSEII.API.DTOs.Pieza3DDTOs;

namespace AppForSEII.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]  
    public class Piezas3DController : ControllerBase
    {

            //used to enable your controller to access to the database private readonly ApplicationDBContext context;
            private readonly ApplicationDbContext _context;
            //used to log any information when your system is running private readonly ILogger <MoviesController> logger;
            private readonly ILogger<Piezas3DController> _logger;


            public Piezas3DController(ApplicationDbContext context, ILogger<Piezas3DController> logger)
            {
                _context = context;
                _logger = logger;
            }

            [HttpGet]
            [Route("[action]")]
            [ProducesResponseType(typeof(IList<Pieza3DSelectDTO>), (int)HttpStatusCode.OK)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<ActionResult> GetPiezas3D(string? nombre, CategoriaPieza? categoria)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var piezas = await _context.Piezas3D
                    .Include(p => p.Material)
                    .Where(p => (nombre == null || p.Nombre.Contains(nombre)) 
                            && (categoria == null || p.Categoria == categoria))
                    .OrderBy(p => p.Nombre)
                    .Select(p => new Pieza3DSelectDTO(
                        p.Id, 
                        p.Nombre, 
                        p.Descripcion, 
                        p.PesoGramos, 
                        p.PrecioEstimado, 
                        p.Categoria,
                        p.Material != null ? p.Material.Nombre : "Sin material"
                    ))
                    .ToListAsync();

                if (piezas.Count == 0)
                {
                    _logger.LogInformation($"{DateTime.Now} No se han encontrado piezas 3D con los filtros especificados");
                    return NotFound("No existen piezas 3D disponibles con los filtros seleccionados.");
                }

                return Ok(piezas);
            }
    }
}
