using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncargosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncargosController> _logger;

        public EncargosController(ApplicationDbContext context, ILogger<EncargosController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("EncargoController initialized");
        }

            [HttpPost]
            [ProducesResponseType(typeof(EncargoDetailsDTO), (int)HttpStatusCode.Created)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<ActionResult> CreateEncargo([FromBody] EncargoCreateDTO encargoDTO)
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"{DateTime.Now} Error: El modelo del encargo no es válido.");
                    return BadRequest("El modelo del encargo no es válido.");
                }

                //fluj alternativo 5: comprobar que se ha incluido al menos una pieza
                if (encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
                {
                    _logger.LogError($"{DateTime.Now} Error: El encargo no contiene líneas.");
                    return BadRequest("Debe incluir al menos una pieza para ser impresa.");
                }

                var encargo = new EncargoImpresion(
                    DateTime.Now, 
                    encargoDTO.NombreCliente, 
                    encargoDTO.ApellidosCliente, 
                    encargoDTO.DireccionEnvio, 
                    encargoDTO.NumeroTelefono, 
                    encargoDTO.Descripcion, 
                    0, 
                    encargoDTO.MetodoPago
                );

            decimal precioTotal = 0;

            foreach (var lineaDto in encargoDTO.LineasEncargo) //por cada producto del encargo
            {
                var pieza = await _context.Piezas3D.FindAsync(lineaDto.Pieza3DId); //buscar la peiza y material en la bbdd
                var material = await _context.Materiales.FindAsync(lineaDto.MaterialId);

                if (pieza == null || material == null)
                {
                    _logger.LogError($"{DateTime.Now} Error: La pieza o el material no existen.");
                    return BadRequest("La pieza o el material seleccionado no existen.");
                }

                var esMaterialValido = await _context.PiezaMaterials //validar si este material es válido para la pieza elegida a traves de PiezaMaterial
                    .AnyAsync(pm => pm.Pieza3DId == lineaDto.Pieza3DId && pm.MaterialId == lineaDto.MaterialId);

                if (!esMaterialValido)
                {  
                    _logger.LogError($"{DateTime.Now} Error: El material '{material.Nombre}' no es válido para la pieza '{pieza.Nombre}'.");  
                    return BadRequest("El material no es válido para la pieza.");
                } 
            

                //Calculamos precios (Precio del material * peso de la pieza
                decimal precioUnidad = (decimal)pieza.PesoGramos * material.PrecioPorGramo;
                decimal subtotal = precioUnidad * lineaDto.Cantidad;
                precioTotal += subtotal;

                //Crear la lineaEncargo vinculada
                var lineaEncargo = new LineaEncargo{
                    Cantidad = lineaDto.Cantidad,
                    PrecioUnidad = precioUnidad,
                    Subtotal = subtotal,
                    Pieza = pieza,
                    MaterialSeleccionado = material,
                    EncargoImpresion = encargo
                };

                encargo.LineasEncargo.Add(lineaEncargo);
            }

            encargo.PrecioTotal = precioTotal;

            _context.EncargosImpresion.Add(encargo);
            await _context.SaveChangesAsync();

            var encargoDetailsDTO = new EncargoDetailsDTO(
                encargo.Id,
                encargo.FechaEncargo,
                encargo.NombreCliente,
                encargo.ApellidosCliente,
                encargo.DireccionEnvio,
                encargo.NumeroTelefono,
                encargo.MetodoPago,
                encargo.Descripcion,
                encargo.PrecioTotal,
                encargo.LineasEncargo.Select(le => new LineaEncargoDetailsDTO(
                    le.Pieza.Nombre,
                    le.MaterialSeleccionado.Nombre,
                    le.Cantidad,
                    le.PrecioUnidad,
                    le.Subtotal,
                    encargo.Descripcion
                )).ToList()
            );


           return CreatedAtAction("GetEncargo", "EncargoDetails", new { id = encargo.Id }, encargoDetailsDTO);
                }
        
    }
}