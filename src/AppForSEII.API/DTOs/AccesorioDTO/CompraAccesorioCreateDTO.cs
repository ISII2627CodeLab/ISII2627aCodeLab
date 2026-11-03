using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTO;

public class LineaCompraAccesorioCreateDTO
{
    public int AccesorioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 1.")]
    public int Cantidad { get; set; }

    public LineaCompraAccesorioCreateDTO()
    {
    }

    public LineaCompraAccesorioCreateDTO(int accesorioId, int cantidad)
    {
        AccesorioId = accesorioId;
        Cantidad = cantidad;
    }

    public override bool Equals(object? obj)
    {
        return obj is LineaCompraAccesorioCreateDTO dTO &&
               AccesorioId == dTO.AccesorioId &&
               Cantidad == dTO.Cantidad;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AccesorioId, Cantidad);
    }
}

public class CompraAccesorioCreateDTO
{
    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ApellidosCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string DireccionEnvio { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string NumeroTelefono { get; set; } = string.Empty;

    public MetodoPago MetodoPago { get; set; }

    [Required]
    public string ClienteId { get; set; } = string.Empty;

    public IList<LineaCompraAccesorioCreateDTO> LineasCompra { get; set; } = new List<LineaCompraAccesorioCreateDTO>();

    public CompraAccesorioCreateDTO()
    {
    }

    public CompraAccesorioCreateDTO(
        string nombreCliente,
        string apellidosCliente,
        string direccionEnvio,
        string numeroTelefono,
        MetodoPago metodoPago,
        string clienteId,
        IList<LineaCompraAccesorioCreateDTO> lineasCompra)
    {
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        MetodoPago = metodoPago;
        ClienteId = clienteId;
        LineasCompra = lineasCompra;
    }

    public override bool Equals(object? obj)
    {
        return obj is CompraAccesorioCreateDTO dTO &&
               NombreCliente == dTO.NombreCliente &&
               ApellidosCliente == dTO.ApellidosCliente &&
               DireccionEnvio == dTO.DireccionEnvio &&
               NumeroTelefono == dTO.NumeroTelefono &&
               MetodoPago == dTO.MetodoPago &&
               ClienteId == dTO.ClienteId &&
               Enumerable.SequenceEqual(LineasCompra, dTO.LineasCompra);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionEnvio, NumeroTelefono, MetodoPago, ClienteId, LineasCompra);
    }
}