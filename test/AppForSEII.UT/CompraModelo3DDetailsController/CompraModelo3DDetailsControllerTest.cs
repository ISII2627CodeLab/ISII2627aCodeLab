using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.CompraModelo3DDetailsController_test
{
    public class GetCompraModelo3D_test : AppForSEII4SqliteUT
    {
        public GetCompraModelo3D_test()
        {
            var licencias = new List<LicenciaModelo3D>() {
                new LicenciaModelo3D("Licencia Estándar", new DateTime(2030, 01, 01)),
            };

            var modelos = new List<Modelo3D>(){
                new Modelo3D("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, licencias[0]),
                new Modelo3D("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, licencias[0]),
            };

            Cliente cliente = new Cliente("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Calle Cervantes 1");

            var compra = new CompraModelo3D(DateTime.Now, "Elena", "Navarro Martínez", "elena@uclm.es",
                "Calle Cervantes 1", "Compra para mi proyecto", 55.50m, MetodoPago.TarjetaCredito, cliente);
            compra.LineasCompra.Add(new LineaCompraModelo(2, 15.00m, compra, modelos[0]));
            compra.LineasCompra.Add(new LineaCompraModelo(1, 25.50m, compra, modelos[1]));

            _context.ApplicationUsers.Add(cliente);
            _context.AddRange(licencias);
            _context.AddRange(modelos);
            _context.Add(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompraModelo3D_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            ILogger<CompraModelo3DController> logger = mock.Object;

            var controller = new CompraModelo3DController(_context, logger);

            //Act
            var result = await controller.GetCompraModelo3D(0);

            //Assert: comprobamos que la respuesta es NotFound (404)
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompraModelo3D_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            ILogger<CompraModelo3DController> logger = mock.Object;
            var controller = new CompraModelo3DController(_context, logger);

            var expectedCompra = new CompraModelo3DDetailDTO(1, DateTime.Now, "Elena", "Navarro Martínez", "elena@uclm.es",
                        "Calle Cervantes 1", MetodoPago.TarjetaCredito, "Compra para mi proyecto", 55.50m,
                        new List<LineaCompraModelo3DDetailDTO>());
            expectedCompra.LineasCompra.Add(new LineaCompraModelo3DDetailDTO(1, "Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, 2, 30.00m, new DateTime(2030, 01, 01)));
            expectedCompra.LineasCompra.Add(new LineaCompraModelo3DDetailDTO(2, "Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, 1, 25.50m, new DateTime(2030, 01, 01)));

            //Act
            var result = await controller.GetCompraModelo3D(1);

            //Assert: comprobamos que la respuesta es OK y obtenemos la compra
            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraDTOActual = Assert.IsType<CompraModelo3DDetailDTO>(okResult.Value);
            //comprobamos que la esperada y la obtenida son iguales
            Assert.Equal(expectedCompra, compraDTOActual);
        }
    }
}
