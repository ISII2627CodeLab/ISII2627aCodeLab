using System;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTOs;

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