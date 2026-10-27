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
        public async Task<ActionResult> GetPiezas3D(string? nombre, string? categoria)
        {
            var piezas = await _context.Piezas3D
                .Where(p => (nombre == null || p.Nombre.Contains(nombre))
                         && (categoria == null || p.Categoria.ToString().Contains(categoria)))
                .OrderBy(p => p.Nombre)
                .Select(p => new Pieza3DSelectDTO(p.Id, p.Nombre, p.PrecioEstimado))
                .ToListAsync();
         
            return Ok(piezas);
        }


    }
}
