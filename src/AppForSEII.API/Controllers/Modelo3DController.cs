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
}