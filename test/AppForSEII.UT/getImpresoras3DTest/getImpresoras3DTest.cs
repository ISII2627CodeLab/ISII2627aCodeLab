using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForMovies.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresora3DDTOs;
using AppForSEII.API.Models;

namespace AppForSEII.UT.Impresora3DControllerTest
{
    public class getImpresoras3DTest : AppForSEII4SqliteUT
    {
        public getImpresoras3DTest()
        {
            // Arrange: Insertamos impresoras de prueba
            var impresoras = new List<Impresora3D>()
            {
                new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m),
                new Impresora3D("Bambu Lab X1", "X1C", TipoImpresora.Polimero, "Impresora cerrada avanzada", 0.25m, 8.0m),
                new Impresora3D("Elegoo Mars 3", "Mars 3", TipoImpresora.Resina, "Impresora de resina UV", 0.30m, 6.0m)
            };

            _context.AddRange(impresoras);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetImpresorasForRental()
        {
            var i1 = new selectImpresora3DDTO(1, "Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m);
            var i2 = new selectImpresora3DDTO(2, "Bambu Lab X1", "X1C", TipoImpresora.Polimero, "Impresora cerrada avanzada", 0.25m, 8.0m);
            var i3 = new selectImpresora3DDTO(3, "Elegoo Mars 3", "Mars 3", TipoImpresora.Resina, "Impresora de resina UV", 0.30m, 6.0m);

            
            var allImpresoras = new List<selectImpresora3DDTO> { i2, i3, i1 };
            var filterByName = new List<selectImpresora3DDTO> { i1 };
            var filterByType = new List<selectImpresora3DDTO> { i3 };

            return new List<object?[]>
            {
                new object?[] { null, null, allImpresoras },                    
                new object?[] { "Prusa", null, filterByName },                  
                new object?[] { null, TipoImpresora.Resina, filterByType },     
            };
        }
 
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetImpresorasForRental))]
        public async Task GetImpresorasForRental_Filters_test(string? nombreFiltro, TipoImpresora? tipoFiltro, List<selectImpresora3DDTO> impresorasEsperadas)
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            ILogger<getImpresoras3D> logger = mock.Object;
            var controller = new getImpresoras3D(_context, logger);

            // Act
            var result = await controller.GetImpresorasForRental(nombreFiltro, tipoFiltro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresorasActual = Assert.IsType<List<selectImpresora3DDTO>>(okResult.Value);
            Assert.Equal(impresorasEsperadas, impresorasActual);
        }
    }
}