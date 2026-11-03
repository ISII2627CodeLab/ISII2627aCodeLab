using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.CompraAccesorioDTOs;

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