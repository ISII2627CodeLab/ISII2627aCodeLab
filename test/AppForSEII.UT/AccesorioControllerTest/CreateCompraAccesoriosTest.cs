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

public class CreateCompraAccesoriosTest : AppForSEII4SqliteUT
{
    private Accesorio _accesorioConStock;
    private Accesorio _accesorioSinStock;
    private Cliente _cliente;

    public CreateCompraAccesoriosTest()
    {
        _cliente = new Cliente("c1", "Maria", "García", "maria@uclm.es", "Calle Real 2");
        _accesorioConStock = new Accesorio("Kit Limpieza", CategoriaAccesorio.KITS_LIMPIEZA, "Universal", 10, 15.0m);
        _accesorioSinStock = new Accesorio("Tubo PTFE", CategoriaAccesorio.REPUESTOS, "Universal", 1, 8.0m);

        _context.Users.Add(_cliente);
        _context.Accesorios.Add(_accesorioConStock);
        _context.Accesorios.Add(_accesorioSinStock);
        _context.SaveChanges();
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_Success_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioConStock.Id, 2)
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        // Act
        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualDTO = Assert.IsType<CompraAccesoriosDetailDTO>(createdResult.Value);
        Assert.Equal(30.0m, actualDTO.PrecioTotal);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_StockInsufficient_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioSinStock.Id, 5) 
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        // Act
        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}