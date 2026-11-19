using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresora3DDTOs;

namespace AppForMovies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class getImpresoras3D : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<getImpresoras3D> _logger;

        public getImpresoras3D(ApplicationDbContext context, ILogger<getImpresoras3D> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Impresora3D>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetImpresoras()
        {
            // Consultamos la tabla usando Entity Framework Core[cite: 14, 17]
            IList<Impresora3D> impresoras = await _context.Impresoras3Ds
                .ToListAsync();
            
            return Ok(impresoras); 
        }

        [HttpGet] 
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<selectImpresora3DDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetImpresorasForRental(string? nombre,TipoImpresora? tipo)
        {
           var impresoras = await _context.Impresoras3Ds
        .Where(m => (nombre == null || m.Nombre.Contains(nombre))
                 && (tipo == null || m.Tipo == tipo))
        .OrderBy(m => m.Nombre)
        .Select(m => new selectImpresora3DDTO(
            m.Id,
            m.Nombre, 
            m.Modelo, 
            m.Tipo,
            m.Descripcion,
            m.PrecioKilovatioHora, 
            m.PrecioReserva
        ))
        .ToListAsync();

        return Ok(impresoras);
        }

       
    }

}