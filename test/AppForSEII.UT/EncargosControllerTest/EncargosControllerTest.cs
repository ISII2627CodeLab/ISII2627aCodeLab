using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;
using System.Net;

namespace AppForSEII.UT.EncargosController_test
{
    public class CreateEncargo_test : AppForSEII4SqliteUT
    {
        public CreateEncargo_test()
        {
            // 1. Creamos el material y la pieza 3D usando tipos explícitos
            Material material = new Material("PLA", 0.05m, 5000.0m);
            Pieza3D pieza3D = new Pieza3D("Dragon Mini", "Miniatura detallada de un dragón", 45.5, 12.50m, CategoriaPieza.MiniaturasYMaquetas);

            // 2. Creamos la relación en la tabla intermedia usando tipo explícito
            PiezaMaterial piezaMaterial = new PiezaMaterial
            {              Pieza3D = pieza3D,
                Material = material
            };

            _context.Materiales.Add(material);
            _context.Piezas3D.Add(pieza3D);
            _context.PiezaMaterials.Add(piezaMaterial);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateEncargo_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;

            var lineaCreateDto = new LineaEncargoCreateDTO(2, 1, 1);//creamos el DTO de entrada para el encargo
            var encargoCreateDto = new EncargoCreateDTO(
                "Juan", 
                "Perez", 
                "Calle Mayor 1", 
                "600123456", 
                "Urgente", 
                MetodoPago.TarjetaCredito, 
                new List<LineaEncargoCreateDTO> { lineaCreateDto }
            );

            
            decimal precioUnidad = (decimal)45.5 * 0.05m;
            decimal subtotal = precioUnidad * 2;

            //preparamos el DTO de detalles esperado (Id = 1 por ser el primer registro guardado)
            var lineaDetailExpected = new LineaEncargoDetailsDTO("Dragon Mini", "PLA", 2, precioUnidad, subtotal, "Urgente");
            var expectedDetails = new EncargoDetailsDTO(
                1,
                DateTime.Now,
                "Juan",
                "Perez",
                "Calle Mayor 1",
                "600123456",
                MetodoPago.TarjetaCredito,
                "Urgente",
                subtotal,
                new List<LineaEncargoDetailsDTO> { lineaDetailExpected }
            );

            var controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.CreateEncargo(encargoCreateDto);

            // Assert
            //primero verificamos que devuelve un codigo HTTP 201 (CreatedAtActionResult)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            
            //luego obtenemos el objeto de los detalles devueltos
            var actualDetails = Assert.IsType<EncargoDetailsDTO>(createdResult.Value);
            expectedDetails.FechaEncargo = actualDetails.FechaEncargo;

            //Comprobamos que los detalles devueltos coinciden con los esperados
            Assert.Equal(expectedDetails, actualDetails);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateEncargo_EmptyLines_Error_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;

            // DTO de entrada con la lista de líneas vacía
            var encargoCreateDto = new EncargoCreateDTO(
                "Juan", 
                "Perez", 
                "Calle Mayor 1", 
                "600123456", 
                "Urgente", 
                MetodoPago.TarjetaCredito, 
                new List<LineaEncargoCreateDTO>() 
            );

            var controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.CreateEncargo(encargoCreateDto);

            // Assert
            //comprobamos que devuelve BadRequest con el mensaje de error esperado
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Debe incluir al menos una pieza para ser impresa.", badRequestResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateEncargo_PiezaOrMaterialNotFound_Error_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;

            // DTO apuntando a Ids que no existen en la base de datos, 999 p.e.
            var lineaCreateDto = new LineaEncargoCreateDTO(1, 999, 999);
            var encargoCreateDto = new EncargoCreateDTO(
                "Juan", 
                "Pérez", 
                "Calle Mayor 1", 
                "600123456", 
                "Urgente", 
                MetodoPago.TarjetaCredito, 
                new List<LineaEncargoCreateDTO> {lineaCreateDto}
            );
            var controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.CreateEncargo(encargoCreateDto);

            // Assert
            // Comprobamos que devuelve BadRequest informando de que no existe
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("La pieza o el material seleccionado no existen.", badRequestResult.Value);
        
        }
    }
}