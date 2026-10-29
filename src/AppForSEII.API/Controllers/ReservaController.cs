using AppForSEII.API.DTOs.Impresora3DDTOs;
namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReservaController> _logger;

    public ReservaController(ApplicationDbContext context, ILogger<ReservaController> logger)
    {
        _context = context;
        _logger = logger;
    }
[HttpPost]
[Route("[action]")]
[ProducesResponseType(typeof(ReservaCreateDTO), (int)HttpStatusCode.Created)] 
[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
public async Task<ActionResult> CreateReserva([FromBody] ReservaCreateDTO reservaDTO)
{
    if (!ModelState.IsValid)
    {
        _logger.LogError($"{DateTime.Now} Exception: ModelState is not valid when creating a reserva");
        return BadRequest("Los datos de la reserva no son válidos.");
    }
    return Created(string.Empty, reservaDTO);
}
}