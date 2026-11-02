using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTOs;

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
    public ICollection<LineaCompraAccesorioDTO> LineasCompra { get; set; } = new List<LineaCompraAccesorioDTO>();

    public CompraAccesoriosDetailDTO()
    {
    }

    public CompraAccesoriosDetailDTO(int id, DateTime fechaCompra, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, decimal precioTotal, MetodoPago metodoPago, ICollection<LineaCompraAccesorioDTO> lineasCompra)
    {
        Id = id;
        FechaCompra = fechaCompra;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        LineasCompra = lineasCompra;
    }
}