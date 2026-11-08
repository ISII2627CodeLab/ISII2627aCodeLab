using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.Modelo3DControllerTest
{
    public class GetModelos3DParaComprarTest : AppForSEII4SqliteUT
    {
        //fechas fijas pare agarantizar los resultaods de las pruebas
        private static readonly DateTime _licenciaVigenteHasta = new DateTime(2030, 1, 1);
        private static readonly DateTime _licenciaCaducadaEn = new DateTime(2020, 1, 1);
        private const string _nombreLicenciaVigente = "Licencia Estandar";
        private const string _nombreLicenciaCaducada = "Licencia Caducada";

        public GetModelos3DParaComprarTest()
        {
            var licenciaVigente = new LicenciaModelo3D(_nombreLicenciaVigente, _licenciaVigenteHasta);
            var licenciaCaducada = new LicenciaModelo3D(_nombreLicenciaCaducada, _licenciaCaducadaEn);
            _context.AddRange(licenciaVigente, licenciaCaducada);
            _context.SaveChanges(); //las guardo antes para que tengan su id cuando se creen los modelos

            var modelos = new List<Modelo3D>()
            {
                new Modelo3D("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, licenciaVigente),  //id 1
                new Modelo3D("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, licenciaVigente),   //id 2
                new Modelo3D("Jarron moderno", "Decoracion", FormatoModelo3D.TresMF, 8.00m, licenciaVigente),  //id 3
                //el de abaj su licencia esta caducada no tiene q salir
                new Modelo3D("Modelo antiguo", "Decoracion", FormatoModelo3D.STL, 5.00m, licenciaCaducada),    //id 4
            };
            _context.AddRange(modelos);
            _context.SaveChanges();
        }

        //casos: filtro de nombre y filtro de categoria con modelos que esperamos recibir
        public static IEnumerable<object?[]> TestCasesFor_GetModelos3DParaComprar()
        {
            var robot = new Modelo3DParaCompraDTO(1, "Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, _nombreLicenciaVigente, _licenciaVigenteHasta);
            var dragon = new Modelo3DParaCompraDTO(2, "Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, _nombreLicenciaVigente, _licenciaVigenteHasta);
            var jarron = new Modelo3DParaCompraDTO(3, "Jarron moderno", "Decoracion", FormatoModelo3D.TresMF, 8.00m, _nombreLicenciaVigente, _licenciaVigenteHasta);

            //el metodo devuelve los modelos ordenados por nombre
            var sinFiltros = new List<Modelo3DParaCompraDTO>() { dragon, jarron, robot };
            var filtroNombre = new List<Modelo3DParaCompraDTO>() { robot };
            var filtroCategoria = new List<Modelo3DParaCompraDTO>() { jarron };   // "Modelo antiguo" también es Decoracion pero está caducado
            var filtroNombreYCategoria = new List<Modelo3DParaCompraDTO>() { dragon };

            var allTests = new List<object?[]>
            {
                new object?[] { null, null, sinFiltros },
                new object?[] { "Robot", null, filtroNombre },
                new object?[] { null, "Decoracion", filtroCategoria },
                new object?[] { "Dragon", "Personajes", filtroNombreYCategoria },
            };
            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetModelos3DParaComprar))]
        public async Task GetModelos3DParaComprar_Filtros_test(string? nombre, string? categoria, List<Modelo3DParaCompraDTO> modelosEsperados)
        {
            //Arrange
            var mock = new Mock<ILogger<Modelo3DController>>();
            ILogger<Modelo3DController> logger = mock.Object;
            var controller = new Modelo3DController(_context, logger);

            //Act
            var result = await controller.GetModelos3DParaComprar(nombre, categoria);

            //Assert
            //comprobamos que la respuesta es OK y obtenemos la lista de modelos
            var okResult = Assert.IsType<OkObjectResult>(result);
            var modelosActual = Assert.IsType<List<Modelo3DParaCompraDTO>>(okResult.Value);
            //comprobamos que la lista esperada y la obtenida son iguales
            Assert.Equal(modelosEsperados, modelosActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_SinResultados_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<Modelo3DController>>();
            ILogger<Modelo3DController> logger = mock.Object;
            var controller = new Modelo3DController(_context, logger);

            //Act: buscamos un nombre que no existe (flujo alternativo 2.2)
            var result = await controller.GetModelos3DParaComprar("No existe.", null);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No existen modelos 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_LicenciaCaducada_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<Modelo3DController>>();
            ILogger<Modelo3DController> logger = mock.Object;
            var controller = new Modelo3DController(_context, logger);

            //Act: La cateoria "Modelo antiguo" existe en la base de datos pero con licencia caducada
            var result = await controller.GetModelos3DParaComprar("Modelo antiguo", null);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No existen modelos 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_FiltrosNoValidos_BadRequest_test()
        {
            //Arrange
            var mock = new Mock<ILogger<Modelo3DController>>();
            ILogger<Modelo3DController> logger = mock.Object;
            var controller = new Modelo3DController(_context, logger);
            controller.ModelState.AddModelError("nombre", "El filtro no es válido");

            //Act
            var result = await controller.GetModelos3DParaComprar("Robot", null);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: los filtros no son válidos.", badRequestResult.Value);
        }
    }
}