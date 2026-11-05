using AppForSEII.API.DTOs.Modelo3DDTOs;
namespace AppForSEII.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompraModelo3DController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CompraModelo3DController> _logger;

    public CompraModelo3DController(ApplicationDbContext context, ILogger<CompraModelo3DController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraModelo3DDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateCompraModelo3D([FromBody] CompraModelo3DCreateDTO compraParaCrear)
        {
            //hay que comprobar ModelState.IsValid por los parametros
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{DateTime.Now} Error: los datos de la compra no son válidos");
                return BadRequest("Error: los datos de la compra no son válidos.");
            }

            // FA 5: el carrito no puede estar vacio
            if (compraParaCrear.LineasCompra.Count == 0)
            {
                _logger.LogError($"{DateTime.Now} Error: la compra no tiene modelos");
                return BadRequest("Error: debes comprar al menos un modelo 3D.");
            }

            // el metodo de pago solo puede ser tarjeta de credito o PayPal
            if (compraParaCrear.MetodoPago != MetodoPago.TarjetaCredito && compraParaCrear.MetodoPago != MetodoPago.Paypal)
            {
                _logger.LogError($"{DateTime.Now} Error: método de pago no válido");
                return BadRequest("Error: el método de pago debe ser tarjeta de crédito o PayPal.");
            }

            //el cliente tiene que estar registrado
            var cliente = await _context.ApplicationUsers.OfType<Cliente>()
                .FirstOrDefaultAsync(c => c.Email == compraParaCrear.CorreoElectronico);
            if (cliente == null)
            {
                _logger.LogError($"{DateTime.Now} Error: no existe un cliente con el correo {compraParaCrear.CorreoElectronico}");
                return BadRequest("Error: el cliente no está registrado.");
            }

            var compra = new CompraModelo3D(
                DateTime.Now,
                compraParaCrear.NombreCliente,
                compraParaCrear.ApellidosCliente,
                compraParaCrear.CorreoElectronico,
                compraParaCrear.DireccionFacturacion,
                compraParaCrear.Descripcion,
                0,
                compraParaCrear.MetodoPago,
                cliente
            );

            foreach (var lineaDto in compraParaCrear.LineasCompra) // por cada modelo de la compra
            {
                // buscar el modelo y su licencia en la base de datos
                var modelo = await _context.Modelos3D
                    .Include(m => m.Licencia)
                    .FirstOrDefaultAsync(m => m.Id == lineaDto.Modelo3DId);

                if (modelo == null)
                {
                    _logger.LogError($"{DateTime.Now} Error: el modelo 3D {lineaDto.Modelo3DId} no existe.");
                    return BadRequest("El modelo 3D seleccionado no existe.");
                }

                var lineaCompra = new LineaCompraModelo(lineaDto.CantidadLicencias, modelo.Precio, compra, modelo);
                compra.LineasCompra.Add(lineaCompra);
            }

            //precio total = suma de los subtotales
            compra.PrecioTotal = compra.LineasCompra.Sum(l => l.Subtotal);

            _context.ComprasModelo3D.Add(compra);
            await _context.SaveChangesAsync();

            //mostrar la compra realizada
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

            return CreatedAtAction("GetModelos3DParaComprar", "CompraModelo3DDetails", new { id = compra.Id }, compraDetailDTO);     

        }
}