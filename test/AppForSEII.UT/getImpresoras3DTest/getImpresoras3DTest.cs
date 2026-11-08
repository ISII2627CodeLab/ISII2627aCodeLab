using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AppForMovies.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresora3DDTOs;
using AppForSEII.API.Models;
using System.Net;

namespace AppForSEII.UT.Impresora3DControllerTest
{
    public class getImpresoras3DTest : AppForSEII4SqliteUT
    {
        public getImpresoras3DTest()
        {
            // Arrange: Insertamos impresoras de prueba en la base de datos SQLite en memoria
            var impresora1 = new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m);
            var impresora2 = new Impresora3D("Bambu Lab X1", "X1C", TipoImpresora.Polimero, "Impresora cerrada avanzada", 0.25m, 8.0m);
            
            _context.Impresoras3Ds.Add(impresora1);
            _context.Impresoras3Ds.Add(impresora2);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task ComputeDivision_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Act
            var result = await controller.ComputeDivision(10m, 2m);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            decimal value = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(5m, value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task ComputeDivision_Error_DivisionByZero_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Act
            var result = await controller.ComputeDivision(10m, 0m);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("op2 must be different from 0", errorMessage);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetImpresoras_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Act
            var result = await controller.GetImpresoras();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresoras = Assert.IsType<List<Impresora3D>>(okResult.Value);
            Assert.NotEmpty(impresoras);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetImpresorasForRental_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Act - Probamos con filtro por nombre y tipo
            var result = await controller.GetImpresorasForRental("Prusa", TipoImpresora.Filamento);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresorasDto = Assert.IsType<List<selectImpresora3DDTO>>(okResult.Value);
            Assert.NotEmpty(impresorasDto);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetImpresorasForRental_WithoutFilters_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Act - Llamamos sin filtros (null) para cubrir las ramas del Where
            var result = await controller.GetImpresorasForRental(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresorasDto = Assert.IsType<List<selectImpresora3DDTO>>(okResult.Value);
            Assert.NotEmpty(impresorasDto);
        }
    }
} 