using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs;
using AppForSEII.API.Models;
using System.Net;

namespace AppForSEII.UT.ReservaControllerTest
{
    public class ReservarControllerTest : AppForSEII4SqliteUT
    {
        public ReservarControllerTest()
        {
            // 1. Creamos la Impresora3D base usando tipos explícitos según el modelo
            Impresora3D impresora3D = new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m);

            // 2. Añadimos la impresora a la base de datos de pruebas SQLite
            _context.Impresoras3Ds.Add(impresora3D);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReserva_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;

            // Creamos el DTO de la línea de reserva
            var lineaCreateDto = new LineaReservaDTO(1, TiempoReserva.UnaHora);
            
            // Creamos el DTO de la cabecera de la reserva
            var reservaCreateDto = new ReservaDTO(
                "Juan", 
                "Pérez", 
                "Calle Mayor 1", 
                MetodoPago.TarjetaCredito, 
                new List<LineaReservaDTO> { lineaCreateDto }
            );

            decimal subtotal = 5.0m;

            // Preparamos los DTOs de detalles esperados con todos los parámetros requeridos
            var lineaDetailExpected = new LineaReservaDetailsDTO(
                "Prusa MK4", 
                "MK4", 
                TipoImpresora.Filamento.ToString(), 
                "Impresora de hilo rápido", 
                TiempoReserva.UnaHora, 
                0.20m, 
                5.0m, 
                subtotal
            );

            var expectedDetails = new ReservaDetailDTO(
                1,
                DateTime.Now,
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                subtotal,
                MetodoPago.TarjetaCredito,
                new List<LineaReservaDetailsDTO> { lineaDetailExpected }
            );

            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.CreateReserva(reservaCreateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDetails = Assert.IsType<ReservaDetailDTO>(createdResult.Value);
            Assert.Equal(expectedDetails, actualDetails);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReserva_EmptyLines_Error_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;

            var reservaCreateDto = new ReservaDTO(
                "Juan", 
                "Pérez", 
                "Calle Mayor 1", 
                MetodoPago.TarjetaCredito, 
                new List<LineaReservaDTO>() 
            );

            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.CreateReserva(reservaCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            Assert.True(problemDetails.Errors.ContainsKey("LineasReserva"));
            Assert.Contains("Debe incluir al menos una línea de reserva.", problemDetails.Errors["LineasReserva"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReserva_ImpresoraNotFound_Error_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;

            var lineaCreateDto = new LineaReservaDTO(999, TiempoReserva.UnaHora);
            var reservaCreateDto = new ReservaDTO(
                "Juan", 
                "Pérez", 
                "Calle Mayor 1", 
                MetodoPago.TarjetaCredito, 
                new List<LineaReservaDTO> { lineaCreateDto }
            );

            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.CreateReserva(reservaCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            Assert.True(problemDetails.Errors.ContainsKey("LineasReserva"));
            Assert.Contains("La impresora seleccionada no existe.", problemDetails.Errors["LineasReserva"]);
        }
    }
}