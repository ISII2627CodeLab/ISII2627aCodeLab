using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.CompraModelo3DControllerTest
{
    public class CreateCompraModelo3DTest : AppForSEII4SqliteUT
    {
        private const string _nombre = "Elena";
        private const string _apellidos = "Navarro Martínez";
        private const string _correo = "elena@uclm.es";
        private const string _direccion = "Calle Cervantes 1";
        private static readonly DateTime _licenciaVigenteHasta = new DateTime(2030, 1, 1);

        public CreateCompraModelo3DTest()
        {
            var licencia = new LicenciaModelo3D("Licencia Estándar", _licenciaVigenteHasta);
            _context.LicenciasModelo3D.Add(licencia);
            _context.SaveChanges(); //se guarda antes para que ya tenga id cuando se creen los modelos
            var modelos = new List<Modelo3D>()
            {
                new Modelo3D("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, licencia),  // Id 1
                new Modelo3D("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, licencia),   // Id 2
            };
            //el constructor de ApplicationUser guarda el userName tambien como email y el controlador busca al cliente por su correo
            var cliente = new Cliente("1", _nombre, _apellidos, _correo, _direccion);
            _context.AddRange(modelos);
            _context.ApplicationUsers.Add(cliente);
            _context.SaveChanges();
        }

        //casos: compra que se intenta crear y mensaje de error que esperamos
        public static IEnumerable<object[]> TestCasesFor_CreateCompraModelo3D_Error()
        {
            var lineasValidas = new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(1, 1) };
            //flujo alternativo 5: carrito vacío
            var carritoVacio = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>());
            //solo se admite tarjeta de credito o PayPal
            var pagoConBizum = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.Bizum, lineasValidas);
            //el correo no corresponde a ningun cliente registrado
            var clienteNoRegistrado = new CompraModelo3DCreateDTO(_nombre, _apellidos, "otro@uclm.es", _direccion, null,
                MetodoPago.Paypal, lineasValidas);
            //el modelo 3D con id 99 no existe en la base de datos
            var modeloInexistente = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(99, 1) });
            var allTests = new List<object[]>
            {
                new object[] { carritoVacio, "Error: debes comprar al menos un modelo 3D." },
                new object[] { pagoConBizum, "Error: el método de pago debe ser tarjeta de crédito o PayPal." },
                new object[] { clienteNoRegistrado, "Error: el cliente no está registrado." },
                new object[] { modeloInexistente, "El modelo 3D seleccionado no existe." },
            };
            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateCompraModelo3D_Error))]
        public async Task CreateCompraModelo3D_Error_test(CompraModelo3DCreateDTO compraDTO, string errorEsperado)
        {
            //Arrange
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            ILogger<CompraModelo3DController> logger = mock.Object;
            var controller = new CompraModelo3DController(_context, logger);
            //Act
            var result = await controller.CreateCompraModelo3D(compraDTO);
            //Assert: comprobamos que la respuesta es BadRequest y obtenemos el error devuelto
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorActual = Assert.IsType<string>(badRequestResult.Value);
            //comprobamos que el error esperado y el obtenido son el mismo
            Assert.Equal(errorEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateCompraModelo3D_Success_test()
        {
            //Arrange
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            ILogger<CompraModelo3DController> logger = mock.Object;
            var controller = new CompraModelo3DController(_context, logger);

            //compra de 2 licencias del Robot (15.00) y 1 del Dragon (25.50)
            var compraDTO = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, "Compra para mi proyecto",
                MetodoPago.TarjetaCredito,
                new List<LineaCompraModelo3DCreateDTO>()
                {
                    new LineaCompraModelo3DCreateDTO(1, 2),
                    new LineaCompraModelo3DCreateDTO(2, 1),
                });

            decimal subtotalRobot = 2 * 15.00m;
            decimal subtotalDragon = 1 * 25.50m;
            decimal precioTotal = subtotalRobot + subtotalDragon;

            //es la primera compra de la base de datos, por eso esperamos id = 1
            var compraEsperada = new CompraModelo3DDetailDTO(1, DateTime.Now, _nombre, _apellidos, _correo, _direccion,
                MetodoPago.TarjetaCredito, "Compra para mi proyecto", precioTotal,
                new List<LineaCompraModelo3DDetailDTO>()
                {
                    new LineaCompraModelo3DDetailDTO("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, 2, subtotalRobot, _licenciaVigenteHasta),
                    new LineaCompraModelo3DDetailDTO("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, 1, subtotalDragon, _licenciaVigenteHasta),
                });

            //Act
            var result = await controller.CreateCompraModelo3D(compraDTO);
            //Assert: comprobamos que la respuesta es 201 Created y que apunta al metodo que muestra los detalles de la compra
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCompraModelo3D", createdResult.ActionName);
            Assert.Equal("CompraModelo3DDetails", createdResult.ControllerName);
            //obtenemos la compra devuelta y comprobamos que es la esperada
            var compraActual = Assert.IsType<CompraModelo3DDetailDTO>(createdResult.Value);
            Assert.Equal(compraEsperada, compraActual);
        }
    }
}