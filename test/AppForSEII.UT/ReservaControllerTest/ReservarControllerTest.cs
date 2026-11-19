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
        private readonly Impresora3D _impresora3D;

        public ReservarControllerTest()
        {
            // 1. Creamos la Impresora3D base usando tipos explícitos según el modelo
            _impresora3D = new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora de hilo rápido", 0.20m, 5.0m);

            // 2. Añadimos la impresora y un cliente a la base de datos de pruebas SQLite
            var cliente = new Cliente("1", "Juan", "Pérez", "juan.perez@uclm.es", "Calle Mayor 1");

            _context.Impresoras3Ds.Add(_impresora3D);
            _context.Users.Add(cliente);
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
            var lineaCreateDto = new LineaReservaDTO(_impresora3D.Id, TiempoReserva.UnaHora);

            // Creamos el DTO de la cabecera de la reserva
            var reservaCreateDto = new ReservaDTO(
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                MetodoPago.TarjetaCredito,
                new List<LineaReservaDTO> { lineaCreateDto }
            );

            decimal subtotal = 5.0m * 1; // factorTiempo = 0 + 1 = 1 -> 5.0 * 1 = 5.0

            // Preparamos los DTOs de líneas esperados usando LineaReservaDTO unificado
            var lineaExpected = new LineaReservaDTO(
                _impresora3D.Id,
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
                DateTime.Today,
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                subtotal,
                MetodoPago.TarjetaCredito,
                new List<LineaReservaDTO> { lineaExpected }
            );

            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.CreateReserva(reservaCreateDto);

            // Assert - Usamos .Result para acceder al CreatedAtActionResult envuelto en el ActionResult<T>
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var actualDetails = Assert.IsType<ReservaDetailDTO>(createdResult.Value);

            Assert.Equal(expectedDetails.NombreCliente, actualDetails.NombreCliente);
            Assert.Equal(expectedDetails.ApellidosCliente, actualDetails.ApellidosCliente);
            Assert.Equal(expectedDetails.DireccionFacturacion, actualDetails.DireccionFacturacion);
            Assert.Equal(expectedDetails.PrecioTotal, actualDetails.PrecioTotal);
            Assert.Equal(expectedDetails.MetodoPago, actualDetails.MetodoPago);
            Assert.Equal(expectedDetails.LineasReserva, actualDetails.LineasReserva);
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

            // Assert - Usamos .Result para acceder al BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("La reserva debe contener al menos una impresora.", errorMessage);
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

            // Assert - Usamos .Result para acceder al BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("La impresora con ID 999 no existe.", errorMessage);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReserva_Error_NombreCorto_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;

            var lineaCreateDto = new LineaReservaDTO(_impresora3D.Id, TiempoReserva.UnaHora);
            var reservaCreateDto = new ReservaDTO(
                "Jo",
                "Pérez",
                "Calle Mayor 1",
                MetodoPago.TarjetaCredito,
                new List<LineaReservaDTO> { lineaCreateDto }
            );

            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.CreateReserva(reservaCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("El cliente no existe.", errorMessage);
        }
    }
} 