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

namespace AppForSEII.API.Controllers;

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

    // 1.GetAccesoriosForPurchase
    /// Consulta y filtra el catálogo de accesorios disponibles para comprar.
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

        var query = _context.Accesorios
            .Where(a => (nombre == null || a.Nombre.Contains(nombre))
                     && (categoria == null || a.Categoria == categoria));

        var accesoriosDTO = await query
            .Select(a => new AccesorioDTO(
                a.Id,
                a.Nombre,
                a.Categoria,
                a.Compatibilidad,
                a.CantidadDisponible,
                a.Precio
            ))
            .ToListAsync();

        if (accesoriosDTO.Count == 0)
        {
            _logger.LogInformation($"{DateTime.Now} No se encontraron accesorios con los filtros especificados.");
            return NotFound("No existen accesorios disponibles con los criterios seleccionados.");
        }

        return Ok(accesoriosDTO);
    }

    // 2.GetCompraAccesorios
    /// Obtiene el detalle completo de una compra de accesorios realizada.
    [HttpGet]
    [Route("[action]/{id}")]
    [ProducesResponseType(typeof(CompraAccesoriosDetailDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetCompraAccesorios(int id)
    {
        var compra = await _context.ComprasAccesorios
            .Include(c => c.LineasCompra)
                .ThenInclude(l => l.Accesorio)
            .Where(c => c.Id == id)
            .Select(c => new CompraAccesoriosDetailDTO(
                c.Id,
                c.FechaCompra,
                c.NombreCliente,
                c.ApellidosCliente,
                c.DireccionEnvio,
                c.NumeroTelefono,
                c.PrecioTotal,
                c.MetodoPago,
                c.LineasCompra.Select(l => new LineaCompraAccesorioDTO(
                    l.Id,
                    l.Cantidad,
                    l.PrecioUnidad,
                    l.Subtotal,
                    l.Accesorio.Nombre
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        if (compra == null)
        {
            _logger.LogWarning($"{DateTime.Now} No se encontró la compra con ID {id}.");
            return NotFound($"No existe ninguna compra de accesorios con el ID {id}.");
        }

        return Ok(compra);
    }

    // 3.CreateCompraAccesorios
    /// Registra una nueva compra de accesorios verificando y descontando el stock.
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(CompraAccesoriosDetailDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> CreateCompraAccesorios([FromBody] CompraAccesorioCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (dto.LineasCompra == null || dto.LineasCompra.Count == 0)
        {
            return BadRequest("El carrito de compra no contiene ningún accesorio.");
        }

        decimal precioTotal = 0m;
        var nuevaCompra = new CompraAccesorios
        {
            FechaCompra = DateTime.Now,
            NombreCliente = dto.NombreCliente,
            ApellidosCliente = dto.ApellidosCliente,
            DireccionEnvio = dto.DireccionEnvio,
            NumeroTelefono = dto.NumeroTelefono,
            MetodoPago = dto.MetodoPago,
            ClienteId = dto.ClienteId
        };

        foreach (var item in dto.LineasCompra)
        {
            var accesorio = await _context.Accesorios.FindAsync(item.AccesorioId);
            if (accesorio == null)
            {
                return NotFound($"El accesorio con ID {item.AccesorioId} no existe.");
            }

            if (accesorio.CantidadDisponible < item.Cantidad)
            {
                _logger.LogWarning($"{DateTime.Now} Stock insuficiente para el accesorio '{accesorio.Nombre}'. Solicitado: {item.Cantidad}, Disponible: {accesorio.CantidadDisponible}");
                return BadRequest($"Stock insuficiente para '{accesorio.Nombre}'. Máximo disponible: {accesorio.CantidadDisponible}.");
            }

            // Descontar stock
            accesorio.CantidadDisponible -= item.Cantidad;

            decimal subtotal = accesorio.Precio * item.Cantidad;
            precioTotal += subtotal;

            var linea = new LineaCompraAccesorio
            {
                Cantidad = item.Cantidad,
                PrecioUnidad = accesorio.Precio,
                Subtotal = subtotal,
                Accesorio = accesorio,
                CompraAccesorios = nuevaCompra
            };

            nuevaCompra.LineasCompra.Add(linea);
        }

        nuevaCompra.PrecioTotal = precioTotal;

        _context.ComprasAccesorios.Add(nuevaCompra);
        await _context.SaveChangesAsync();

        var resultDTO = new CompraAccesoriosDetailDTO(
            nuevaCompra.Id,
            nuevaCompra.FechaCompra,
            nuevaCompra.NombreCliente,
            nuevaCompra.ApellidosCliente,
            nuevaCompra.DireccionEnvio,
            nuevaCompra.NumeroTelefono,
            nuevaCompra.PrecioTotal,
            nuevaCompra.MetodoPago,
            nuevaCompra.LineasCompra.Select(l => new LineaCompraAccesorioDTO(
                l.Id,
                l.Cantidad,
                l.PrecioUnidad,
                l.Subtotal,
                l.Accesorio.Nombre
            )).ToList()
        );

        return CreatedAtAction(nameof(GetCompraAccesorios), new { id = nuevaCompra.Id }, resultDTO);
    }
}