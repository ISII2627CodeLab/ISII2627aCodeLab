using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class CompraAccesorios
{
    public CompraAccesorios()
    {
    }

    public CompraAccesorios(
        int id, 
        DateTime fechaCompra, 
        string nombreCliente, 
        string apellidosCliente, 
        string direccionEnvio, 
        string numeroTelefono, 
        decimal precioTotal, 
        MetodoPago metodoPago, 
        Cliente? cliente = null)
    {
        Id = id;
        FechaCompra = fechaCompra;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;

        if (cliente != null)
        {
            Cliente = cliente;
            ClienteId = cliente.Id;
        }
    }

    public int Id { get; set; }

    public DateTime FechaCompra { get; set; }

    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [StringLength(100)]
    public string ApellidosCliente { get; set; } = string.Empty;

    [StringLength(200)]
    public string DireccionEnvio { get; set; } = string.Empty;

    [StringLength(20)]
    public string NumeroTelefono { get; set; } = string.Empty;

    public decimal PrecioTotal { get; set; }

    public MetodoPago MetodoPago { get; set; }

    // Relacion opcional con Client (hereda de ApplicationUser por lo que su ID es string)
    public string? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    // Coleccion de lineas de compra (relación composición 1 a 1..*)
    public ICollection<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();
}