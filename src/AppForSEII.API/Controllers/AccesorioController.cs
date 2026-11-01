using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.AccesorioDTOs;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesorioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccesorioController> _logger;

        public AccesorioController(ApplicationDbContext context, ILogger<AccesorioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Método de prueba original que esta en el enunciado del seminario
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
        }

        //Método de Comprar Accesorios
        ///Obtiene el catálogo de accesorios disponibles con opción de filtrado por nombre y categoría.
        ///<param name="nombre">Nombre del accesorio (filtro opcional)</param>
        ///<param name="categoria">Categoria del accesorio (filtro opcional)</param>
        ///<returns>Lista de accesorios en formato DTO</returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<AccesorioDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAccesoriosForPurchase(string? nombre, CategoriaAccesorio? categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Filtrado de losparámetros
            var accesoriosQuery = _context.Accesorios
                .Where(a => (nombre == null || a.Nombre.Contains(nombre))
                         && (categoria == null || a.Categoria == categoria));

            // Llamada hacia AccesorioDTO
            var accesoriosDTO = await accesoriosQuery
                .Select(a => new AccesorioDTO(
                    a.Id,
                    a.Nombre,
                    a.Categoria,
                    a.Compatibilidad,
                    a.CantidadDisponible,
                    a.Precio
                ))
                .ToListAsync();

            // Flujo Alternativo 2.1: Notificar si no existen accesorios disponibles con los criterios introducidos
            if (accesoriosDTO.Count == 0)
            {
                _logger.LogInformation($"{DateTime.Now} No se encontraron accesorios con los filtros especificados.");
                return NotFound("No existen accesorios disponibles con los criterios seleccionados.");
            }

            return Ok(accesoriosDTO);
        }
    }
}