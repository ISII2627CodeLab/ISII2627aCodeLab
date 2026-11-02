using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTOs;

public class LineaCompraAccesorioCreateDTO
{
    [Required]
    public int AccesorioId { get; set; }

    [Required]
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

    [Required]
    public MetodoPago MetodoPago { get; set; }

    public string? ClienteId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Debe incluir al menos una línea de compra.")]
    public IList<LineaCompraAccesorioCreateDTO> LineasCompra { get; set; } = new List<LineaCompraAccesorioCreateDTO>();

    public CompraAccesorioCreateDTO()
    {
    }
}