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
        [ProducesResponseType(typeof(EncargoCreateDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateEncargo([FromBody] EncargoCreateDTO encargoDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{DateTime.Now} Error: El modelo del encargo no es válido.");
                return BadRequest(ModelState);
            }

            //Fluj alternativo 5: comprobar que se ha incluido al menos una pieza
            if (encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
            {
                return BadRequest(new { message = "Debe incluir al menos una pieza para ser impresa." });
            }

            var encargo = new EncargoImpresion
            {
                DireccionEnvio = encargoDTO.DireccionEnvio,
                NombreCliente = encargoDTO.NombreCliente,
                ApellidosCliente = encargoDTO.ApellidosCliente,
                NumeroTelefono = encargoDTO.NumeroTelefono,
                MetodoPago = encargoDTO.MetodoPago,
                Descripcion = encargoDTO.Descripcion,
                FechaEncargo = DateTime.Now,
                LineasEncargo = new List<LineaEncargo>()
            };

            decimal precioTotal = 0;

            foreach (var lineaDto in encargoDTO.LineasEncargo) //por cada producto del encargo
            {
                var pieza = await _context.Piezas3D.FindAsync(lineaDto.Pieza3DId); //buscar la peiza y material en la bbdd
                var material = await _context.Materiales.FindAsync(lineaDto.MaterialId);

                if (pieza == null || material == null)
                {
                    return BadRequest(new { message = "La pieza o el material seleccionado no existen." });
                }

               // var esMaterialValido = await _context.PiezaMaterials //validar si este material es válido para la pieza elegida a traves de PiezaMaterial
                //    .AnyAsync(pm => pm.Pieza3DId == lineaDto.Pieza3DId && pm.MaterialId == lineaDto.MaterialId);

                //if (!esMaterialValido)
                //{
                //    return BadRequest(new { message = $"El material '{material.Nombre}' no es válido para la pieza '{pieza.Nombre}'." });
                //}

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

            return CreatedAtAction(nameof(CreateEncargo), new { id = encargo.Id }, encargoDTO);
                }
            

    }
}