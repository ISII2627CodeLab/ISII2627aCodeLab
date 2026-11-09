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
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioConStock.Id, 2)
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualDTO = Assert.IsType<CompraAccesoriosDetailDTO>(createdResult.Value);
        Assert.Equal(30.0m, actualDTO.PrecioTotal);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_StockInsufficient_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioSinStock.Id, 5) 
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_EmptyCart_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>(); 

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("El carrito de compra no contiene ningún accesorio.", badRequestResult.Value);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_AccesorioNotFound_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(999, 1)
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("no existe", notFoundResult.Value!.ToString());
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_ClienteNotFound_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioConStock.Id, 1)
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "cliente_falso", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("registrado con el ID", notFoundResult.Value!.ToString());
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_InvalidModelState_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);
        
        controller.ModelState.AddModelError("NombreCliente", "El nombre del cliente es obligatorio.");

        var lineasCreate = new List<LineaCompraAccesorioCreateDTO>
        {
            new LineaCompraAccesorioCreateDTO(_accesorioConStock.Id, 1)
        };

        var purchaseDTO = new CompraAccesorioCreateDTO(
            "", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", lineasCreate
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task CreateCompraAccesorios_NullCart_test()
    {
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        // Pasamos explícitamente null a la lista de líneas para cubrir la primera condición del if
        var purchaseDTO = new CompraAccesorioCreateDTO(
            "Maria", "García", "Calle Real 2", "611223344", MetodoPago.TarjetaCredito, "c1", null!
        );

        var result = await controller.CreateCompraAccesorios(purchaseDTO);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("El carrito de compra no contiene ningún accesorio.", badRequestResult.Value);
    }
}