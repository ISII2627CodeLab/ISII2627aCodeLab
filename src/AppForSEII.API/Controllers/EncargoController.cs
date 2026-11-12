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
        }

            [HttpPost]
            [ProducesResponseType(typeof(EncargoDetailsDTO), (int)HttpStatusCode.Created)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
            public async Task<ActionResult> CreateEncargo([FromBody] EncargoCreateDTO encargoDTO)
            {
                if (encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
                {
                    _logger.LogError($"{DateTime.Now} Error: El encargo no contiene lineas.");
                    ModelState.AddModelError("LineasEncargo", "Debe incluir al menos una pieza para ser impresa.");
                }

            //añadido examen
            if (!encargoDTO.DireccionEnvio.EndsWith("España"))
            {
                _logger.LogError($"{DateTime.Now} Error: Solo se realizan envíos dentro de España");
                ModelState.AddModelError("DireccionEnvio", "¡Error! Solo se realizan envíos dentro de España");
            } //hasta aqui examen

                else{
                    foreach (var lineaDto in encargoDTO.LineasEncargo) //por cada producto del encargo
                    {
                        var pieza = await _context.Piezas3D.FindAsync(lineaDto.Pieza3DId); //buscar la peiza y material en la bbdd
                        var material = await _context.Materiales.FindAsync(lineaDto.MaterialId);

                        if (pieza == null || material == null)
                        {
                            _logger.LogError($"{DateTime.Now} Error: La pieza o el material no existen.");
                            ModelState.AddModelError("LineasEncargo", "La pieza o el material seleccionado no existen.");
                            break;
                        }

                        var esMaterialValido = await _context.PiezaMaterials //validar si este material es valido para la pieza elegida a traves de PiezaMaterial
                            .AnyAsync(pm => pm.Pieza3DId == lineaDto.Pieza3DId && pm.MaterialId == lineaDto.MaterialId);

                        if (!esMaterialValido)
                        {  
                            _logger.LogError($"{DateTime.Now} Error: El material '{material.Nombre}' no es válido para la pieza '{pieza.Nombre}'.");  
                            ModelState.AddModelError("LineasEncargo", "El material no es válido para la pieza.");
                        } 
                    }
                }
                

                // Si hay errores acumulados, se devuelven todos juntos, valida todos
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"{DateTime.Now} Error: El modelo del encargo no es válido.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
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

            
            

            foreach (var lineaDto in encargoDTO.LineasEncargo!) //por cada producto del encargo
            {
                var pieza = await _context.Piezas3D.FindAsync(lineaDto.Pieza3DId); //buscar la pieza y material en la bbdd
                var material = await _context.Materiales.FindAsync(lineaDto.MaterialId);

                decimal precioUnidad = (decimal)pieza!.PesoGramos * material!.PrecioPorGramo;
                decimal subtotal = precioUnidad * lineaDto.Cantidad;
                precioTotal += subtotal;

                // Crear la lineaEncargo vinculada
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