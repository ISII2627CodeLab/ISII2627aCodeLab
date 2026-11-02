using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTO;

public class LineaCompraAccesorioDTO
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }
    public string AccesorioNombre { get; set; } = string.Empty;

    public LineaCompraAccesorioDTO()
    {
    }

    public LineaCompraAccesorioDTO(int id, int cantidad, decimal precioUnidad, decimal subtotal, string accesorioNombre)
    {
        Id = id;
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
        AccesorioNombre = accesorioNombre;
    }

    public override bool Equals(object? obj)
    {
        return obj is LineaCompraAccesorioDTO dTO &&
               Id == dTO.Id &&
               Cantidad == dTO.Cantidad &&
               PrecioUnidad == dTO.PrecioUnidad &&
               Subtotal == dTO.Subtotal &&
               AccesorioNombre == dTO.AccesorioNombre;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Cantidad, PrecioUnidad, Subtotal, AccesorioNombre);
    }
}

public class CompraAccesoriosDetailDTO
{
    public int Id { get; set; }
    public DateTime FechaCompra { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string ApellidosCliente { get; set; } = string.Empty;
    public string DireccionEnvio { get; set; } = string.Empty;
    public string NumeroTelefono { get; set; } = string.Empty;
    public decimal PrecioTotal { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public ICollection<LineaCompraAccesorioDTO> LineasCompra { get; set; } = new List<LineaCompraAccesorioDTO>();

    public CompraAccesoriosDetailDTO()
    {
    }

    public CompraAccesoriosDetailDTO(
        int id,
        DateTime fechaCompra,
        string nombreCliente,
        string apellidosCliente,
        string direccionEnvio,
        string numeroTelefono,
        decimal precioTotal,
        MetodoPago metodoPago,
        string clienteId,
        ICollection<LineaCompraAccesorioDTO> lineasCompra)
    {
        Id = id;
        FechaCompra = fechaCompra;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        ClienteId = clienteId;
        LineasCompra = lineasCompra;
    }

    public override bool Equals(object? obj)
    {
        return obj is CompraAccesoriosDetailDTO dTO &&
               Id == dTO.Id &&
               FechaCompra.Equals(dTO.FechaCompra) &&
               NombreCliente == dTO.NombreCliente &&
               ApellidosCliente == dTO.ApellidosCliente &&
               DireccionEnvio == dTO.DireccionEnvio &&
               NumeroTelefono == dTO.NumeroTelefono &&
               PrecioTotal == dTO.PrecioTotal &&
               MetodoPago == dTO.MetodoPago &&
               ClienteId == dTO.ClienteId &&
               Enumerable.SequenceEqual(LineasCompra, dTO.LineasCompra);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(FechaCompra);
        hash.Add(NombreCliente);
        hash.Add(ApellidosCliente);
        hash.Add(DireccionEnvio);
        hash.Add(NumeroTelefono);
        hash.Add(PrecioTotal);
        hash.Add(MetodoPago);
        hash.Add(ClienteId);
        return hash.ToHashCode();
    }
}