using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.DTOs.Pieza3DDTOs;
using AppForSEII.API.Models;


namespace AppForSEII.UT.Pieza3DControllerTest
{
    public class getPiezas3DparaEncargarTest : AppForSEII4SqliteUT
    {
        public getPiezas3DparaEncargarTest()
        {
            var material1 = new Material("PLA", 0.05m, 5000.0m);
            var material2 = new Material("ABS", 0.07m, 3000.0m);
            var piezas3D = new List<Pieza3D>(){
                new Pieza3D("Dragon Mini", "Miniatura detallada de un dragón", 45.5, 12.50m, CategoriaPieza.MiniaturasYMaquetas),
                new Pieza3D("Soporte Móvil", "Soporte de escritorio", 150.0, 8.99m, CategoriaPieza.HerramientasYAccesorios),
                new Pieza3D("Vaso de Adorno", "Pieza decorativa para recipientes", 20.0, 4.50m, CategoriaPieza.Decoracion)
            } ;

        _context.AddRange(piezas3D);
        _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetPiezas3D()
        {
            var p1 = new Pieza3DSelectDTO(1, "Dragon Mini", "Miniatura detallada de un dragón", 45.5, 12.50m, CategoriaPieza.MiniaturasYMaquetas, "PLA");
            var p2 = new Pieza3DSelectDTO(2, "Soporte Móvil", "Soporte de escritorio", 150.0, 8.99m, CategoriaPieza.HerramientasYAccesorios, "ABS");
            var p3 = new Pieza3DSelectDTO(3, "Vaso de Adorno", "Pieza decorativa para recipientes", 20.0, 4.50m, CategoriaPieza.Decoracion, "PLA");

            var allPieces = new List<Pieza3DSelectDTO> { p1, p2, p3 };
            var filterByName = new List<Pieza3DSelectDTO> { p1 }; 
            var filterByCategory = new List<Pieza3DSelectDTO> { p3 }; 
            var filterByBoth = new List<Pieza3DSelectDTO> { p3 };

            return new List<object?[]>
            {
                new object?[] { null, null, allPieces },                  // Caso1: Sin filtros devolveria todas ordenadas
                new object?[] { "Dragon", null, filterByName },          // Caso 2: si fiiltras por nombre
                new object?[] { null, CategoriaPieza.Decoracion, filterByCategory }, // Caso 3: si filtras por categoría
                new object?[] { "Vaso", CategoriaPieza.Decoracion, filterByBoth }     // Caso 4: Ambos filtros a la vez
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPiezas3D))]
        public async Task GetPiezas3D_Filters_test(string? nombreFiltro, CategoriaPieza? categoriaFiltro, List<Pieza3DSelectDTO> piezasEsperadas)
        {
            // Arrange
            var mock = new Mock<ILogger<Piezas3DController>>();
            ILogger<Piezas3DController> logger = mock.Object;
            var controller = new Piezas3DController(_context, logger);

            // Act
            var result = await controller.GetPiezas3D(nombreFiltro, categoriaFiltro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezasActual = Assert.IsType<List<Pieza3DSelectDTO>>(okResult.Value);
            Assert.Equal(piezasEsperadas, piezasActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPiezas3D_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<Piezas3DController>>();
            ILogger<Piezas3DController> logger = mock.Object;
            var controller = new Piezas3DController(_context, logger);

            // Act -> Buscamos un nombre que no exista para forzar el NotFound
            var result = await controller.GetPiezas3D("PiezaInexistente", null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No existen piezas 3D disponibles con los filtros seleccionados.", notFoundResult.Value);
        }
    }
}