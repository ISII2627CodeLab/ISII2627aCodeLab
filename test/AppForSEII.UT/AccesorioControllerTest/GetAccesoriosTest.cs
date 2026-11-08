using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.AccesorioDTO;

namespace AppForSEII.UT.AccesorioController_test;

public class GetAccesoriosTest : AppForSEII4SqliteUT
{
    private Accesorio _accesorio1;
    private Accesorio _accesorio2;

    public GetAccesoriosTest()
    {
        _accesorio1 = new Accesorio("Boquilla Latón 0.4mm", CategoriaAccesorio.BOQUILLAS, "Ender 3", 10, 5.99m);
        _accesorio2 = new Accesorio("Base Vidrio Templado", CategoriaAccesorio.BASES_IMPRESION, "Universal", 5, 20.00m);

        _context.Accesorios.AddRange(_accesorio1, _accesorio2);
        _context.SaveChanges(); 
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetAccesoriosForPurchase_Success_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var expectedDTOs = new List<AccesorioDTO>
        {
            new AccesorioDTO(_accesorio1.Id, "Boquilla Latón 0.4mm", CategoriaAccesorio.BOQUILLAS, "Ender 3", 10, 5.99m)
        };

        // Act
        var result = await controller.GetAccesoriosForPurchase("Boquilla", null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDTOs = Assert.IsType<List<AccesorioDTO>>(okResult.Value);
        Assert.Equal(expectedDTOs, actualDTOs);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetAccesoriosForPurchase_NotFound_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        // Act
        var result = await controller.GetAccesoriosForPurchase("Inexistente", null);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}