using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.AccesorioDTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppForSEII.UT.AccesorioController_test;

public class GetCompraAccesoriosTest : AppForSEII4SqliteUT
{
    private CompraAccesorios _compra;
    private Accesorio _accesorio;
    private Cliente _cliente;

    public GetCompraAccesoriosTest()
    {
        _cliente = new Cliente("c1", "Juan", "Pérez", "juan@uclm.es", "Calle Mayor 1");
        _accesorio = new Accesorio("Base Vidrio", CategoriaAccesorio.BASES_IMPRESION, "Universal", 5, 20.0m);
        var fecha = new DateTime(2026, 5, 10);
        
        _compra = new CompraAccesorios(fecha, "Juan", "Pérez", "Calle Mayor 1", "600123456", 20.0m, MetodoPago.Bizum, _cliente);
        var linea = new LineaCompraAccesorio(1, 20.0m, 20.0m, _accesorio, _compra);
        _compra.LineasCompra.Add(linea);

        _context.Users.Add(_cliente);
        _context.Accesorios.Add(_accesorio);
        _context.ComprasAccesorios.Add(_compra);
        
        _context.SaveChanges(); 
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetCompraAccesorios_Success_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        var lineaGenerada = System.Linq.Enumerable.First(_compra.LineasCompra);
        
        var lineasExpected = new List<LineaCompraAccesorioDTO>
        {
            new LineaCompraAccesorioDTO(lineaGenerada.Id, 1, 20.0m, 20.0m, "Base Vidrio")
        };

        var expectedDetail = new CompraAccesoriosDetailDTO(
            _compra.Id, new DateTime(2026, 5, 10), "Juan", "Pérez", "Calle Mayor 1", "600123456", 20.0m, MetodoPago.Bizum, "c1", lineasExpected
        );

        // Act
        var result = await controller.GetCompraAccesorios(_compra.Id); 

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetail = Assert.IsType<CompraAccesoriosDetailDTO>(okResult.Value);
        Assert.Equal(expectedDetail, actualDetail);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetCompraAccesorios_NotFound_test()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        // Act
        var result = await controller.GetCompraAccesorios(0);

        // Assert
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        var BadRequestResult = Assert.IsType<NotFoundObjectResult>(okResult);
        Assert.Equal("¡Error! No existe ningun elemento con ese ID 0.", BadRequestResult.Value);
    }
    [Theory]
    [InlineData(-1)]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetCompraAccesorios_BadRequest_test(int invalidID)
    {
        // Arrange
        var mockLogger = new Mock<ILogger<AccesorioController>>();
        var controller = new AccesorioController(_context, mockLogger.Object);

        // Act
        var result = await controller.GetCompraAccesorios(invalidID);

        // Assert
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        var BadRequestResult = Assert.IsType<BadRequestObjectResult>(okResult);
        Assert.Equal("¡Error! El ID: -1 no puede ser.", BadRequestResult.Value);
    }
}