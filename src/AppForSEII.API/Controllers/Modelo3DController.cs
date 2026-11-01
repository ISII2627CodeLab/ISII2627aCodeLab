using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Modelo3DController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<Modelo3DController> _logger;

    public Modelo3DController(ApplicationDbContext context, ILogger<Modelo3DController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(typeof(IList<Modelo3DParaCompraDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetModelos3DParaComprar(string? nombre, string? categoria)
    {
        var hoy = DateTime.Now;
        var modelos = await _context.Modelos3D
            .Include(m => m.Licencia)                                  
            .Where(m => (nombre == null || m.Nombre.Contains(nombre))   
                        && (categoria == null || m.Categoria.Equals(categoria)) 
                        && m.Licencia.FechaExpiracion > hoy)
            .OrderBy(m => m.Nombre)                                    
            .Select(m => new Modelo3DParaCompraDTO(m.Id, m.Nombre, m.Categoria,
                                               m.Formato, m.Precio, m.Licencia.Nombre, m.Licencia.FechaExpiracion))
            .ToListAsync();

        return Ok(modelos);
    }

}