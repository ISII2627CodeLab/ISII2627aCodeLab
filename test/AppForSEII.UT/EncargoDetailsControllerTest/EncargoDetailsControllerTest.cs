using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.EncargoDTOs;


namespace AppForSEII.UT.EncargoDetailsControllerTest
{
    public class GetEncargoDetailsTest : AppForSEII4SqliteUT
    {
        public GetEncargoDetailsTest()
        {
            //insertamos los datos necesarios en la BD de prueba 
            // para que el Include y el where del controlador los encuentren al buscar por ID = 1.
            var material = new Material("PLA", 0.05m, 5000.0m);
            var pieza3D = new Pieza3D("Dragon Mini", "Miniatura detallada de un dragón", 45.5, 12.50m, CategoriaPieza.MiniaturasYMaquetas);
            
            var encargo = new EncargoImpresion(
                new DateTime(2026, 5, 10, 10, 0, 0),
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                "600123456",
                "Urgente",
                4.55m,
                MetodoPago.TarjetaCredito
            );

            decimal precioUnidad = (decimal)45.5 * 0.05m; // 2.275
            decimal subtotal = precioUnidad * 2;         // 4.55

            LineaEncargo lineaEncargo = new LineaEncargo(2, precioUnidad, subtotal)
            {
                Pieza = pieza3D,
                MaterialSeleccionado = material,
                EncargoImpresion = encargo
            };
            encargo.LineasEncargo.Add(lineaEncargo);

            _context.Materiales.Add(material);
            _context.Piezas3D.Add(pieza3D);
            _context.EncargosImpresion.Add(encargo);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetEncargo_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargoDetailsController>>();
            ILogger<EncargoDetailsController> logger = mock.Object;

            decimal precioUnidad = (decimal)45.5 * 0.05m;
            decimal subtotal = precioUnidad * 2;

            //preparar el DTO de detalles esperado
            var lineaDetailExpected = new LineaEncargoDetailsDTO("Dragon Mini", "PLA", 2, precioUnidad, subtotal, "Urgente");
            var expectedDetails = new EncargoDetailsDTO(
                1,
                new DateTime(2026, 5, 10, 10, 0, 0),
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                "600123456",
                MetodoPago.TarjetaCredito,
                "Urgente",
                subtotal,
                new List<LineaEncargoDetailsDTO> {lineaDetailExpected}
            );

            var controller = new EncargoDetailsController(_context, logger);

            // Act
            var result = await controller.GetEncargo(1);

            // Assert
            //1.verificamos que devuelve un OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            
            //2.Extraemos el valor y comprobamos que es del tipo EncargoDetailsDTO
            var actualDetails = Assert.IsType<EncargoDetailsDTO>(okResult.Value);

            //comprobamos que es igual al objeto esperado
            Assert.Equal(expectedDetails, actualDetails);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetEncargo_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargoDetailsController>>();
            ILogger<EncargoDetailsController> logger = mock.Object;

            var controller = new EncargoDetailsController(_context, logger);

            // Act: Buscamos un ID que no existe en la BD de prueba, 99 p.e.
            var result = await controller.GetEncargo(99);

            // Assert
            // Verificamos que devuelve un NotFoundResult
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}