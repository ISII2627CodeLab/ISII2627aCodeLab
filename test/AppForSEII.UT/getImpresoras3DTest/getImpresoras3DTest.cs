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
        private readonly Impresora3D _impresora1;
        private readonly Impresora3D _impresora2;

        public getImpresoras3DTest()
        {
            // Arrange: Insertamos impresoras de prueba en la base de datos SQLite en memoria
            _impresora1 = new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m);
            _impresora2 = new Impresora3D("Bambu Lab X1", "X1C", TipoImpresora.Polimero, "Impresora cerrada avanzada", 0.25m, 8.0m);

            _context.Impresoras3Ds.Add(_impresora1);
            _context.Impresoras3Ds.Add(_impresora2);
            _context.SaveChanges();
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
            Assert.Equal(2, impresoras.Count);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetImpresorasForRental_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            var expectedImpresoras = new List<selectImpresora3DDTO>
            {
                new selectImpresora3DDTO(
                    _impresora1.Id,
                    _impresora1.Nombre,
                    _impresora1.Modelo,
                    _impresora1.Tipo,
                    _impresora1.Descripcion,
                    _impresora1.PrecioKilovatioHora,
                    _impresora1.PrecioReserva
                )
            };

            // Act - Probamos con filtro por nombre y tipo
            var result = await controller.GetImpresorasForRental("Prusa", TipoImpresora.Filamento);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresorasDto = Assert.IsType<List<selectImpresora3DDTO>>(okResult.Value);
            Assert.Equal(expectedImpresoras, impresorasDto);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetImpresorasForRental_WithoutFilters_test()
        {
            // Arrange
            var mock = new Mock<ILogger<getImpresoras3D>>();
            var controller = new getImpresoras3D(_context, mock.Object);

            // Ordenadas por Nombre ascendente: "Bambu Lab X1" antes que "Prusa MK4"
            var expectedImpresoras = new List<selectImpresora3DDTO>
            {
                new selectImpresora3DDTO(
                    _impresora2.Id,
                    _impresora2.Nombre,
                    _impresora2.Modelo,
                    _impresora2.Tipo,
                    _impresora2.Descripcion,
                    _impresora2.PrecioKilovatioHora,
                    _impresora2.PrecioReserva
                ),
                new selectImpresora3DDTO(
                    _impresora1.Id,
                    _impresora1.Nombre,
                    _impresora1.Modelo,
                    _impresora1.Tipo,
                    _impresora1.Descripcion,
                    _impresora1.PrecioKilovatioHora,
                    _impresora1.PrecioReserva
                )
            };

            // Act - Llamamos sin filtros (null) para cubrir las ramas del Where
            var result = await controller.GetImpresorasForRental(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var impresorasDto = Assert.IsType<List<selectImpresora3DDTO>>(okResult.Value);
            Assert.Equal(expectedImpresoras, impresorasDto);
        }
    }
} 