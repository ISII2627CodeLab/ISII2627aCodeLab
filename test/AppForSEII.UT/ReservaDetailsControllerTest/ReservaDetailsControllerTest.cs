using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs;
using AppForSEII.API.Models;

namespace AppForSEII.UT.ReservaDetailsControllerTest
{
    public class ReservaDetailsControllerTest : AppForSEII4SqliteUT
    {
        public ReservaDetailsControllerTest()
        {
            // Insertamos los datos necesarios en la BD de prueba 
            var impresora = new Impresora3D("Prusa MK4", "MK4", TipoImpresora.Filamento, "Impresora rápida", 0.20m, 5.0m);
            
            var reserva = new ReservaImpresora(
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                10.0m,
                MetodoPago.TarjetaCredito
            );

            var lineaReserva = new LineaReserva(TiempoReserva.DosHoras, 10.0m, impresora.Id)
            {
                Impresora3D = impresora
            };
            reserva.LineaReservas = new List<LineaReserva> { lineaReserva };

            _context.Impresoras3Ds.Add(impresora);
            _context.ReservaImpresoras.Add(reserva);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReservaDetails_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaDetailsController>>();
            ILogger<ReservaDetailsController> logger = mock.Object;

            var lineaDetailExpected = new LineaReservaDetailsDTO(
                "Prusa MK4", 
                "MK4", 
                "Filamento", 
                "Impresora rápida", 
                TiempoReserva.DosHoras, 
                0.20m, 
                5.0m, 
                10.0m
            );

            // Obtenemos la fecha real de la reserva insertada para que coincida perfectamente
            var fechaReservaReal = _context.ReservaImpresoras.First().FechaReserva;

            var expectedDetails = new ReservaDetailDTO(
                1,
                fechaReservaReal,
                "Juan",
                "Pérez",
                "Calle Mayor 1",
                10.0m,
                MetodoPago.TarjetaCredito,
                new List<LineaReservaDetailsDTO> { lineaDetailExpected }
            );

            var controller = new ReservaDetailsController(_context, logger);

            // Act
            var result = await controller.GetReservaDetails(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualDetails = Assert.IsType<ReservaDetailDTO>(okResult.Value);

            Assert.Equal(expectedDetails.Id, actualDetails.Id);
            Assert.Equal(expectedDetails.NombreCliente, actualDetails.NombreCliente);
            Assert.Equal(expectedDetails.ApellidosCliente, actualDetails.ApellidosCliente);
            Assert.Equal(expectedDetails.PrecioTotal, actualDetails.PrecioTotal);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReservaDetails_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaDetailsController>>();
            ILogger<ReservaDetailsController> logger = mock.Object;

            var controller = new ReservaDetailsController(_context, logger);

            // Act
            var result = await controller.GetReservaDetails(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
} 