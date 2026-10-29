using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models; // Asegúrate de tener este using para los enums

namespace AppForSEII.API.DTOs.Impresora3DDTOs;

public class ReservaCreateDTO
{
    public ReservaCreateDTO(string nombreCliente, string apellidosCliente, string direccionFacturacion, MetodoPago metodoPago, int impresora3DId, TiempoReserva tiempoReserva)
    {
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        MetodoPago = metodoPago;
        Impresora3DId = impresora3DId;
        TiempoReserva = tiempoReserva;
    }

    [Required]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string NombreCliente { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Los apellidos no pueden superar los 50 caracteres.")]
    public string ApellidosCliente { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "La dirección de facturación no puede superar los 100 caracteres.")]
    public string DireccionFacturacion { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    public int Impresora3DId { get; set; }

    [Required]
    public TiempoReserva TiempoReserva { get; set; }

    // Método Equals requerido para las pruebas automáticas
    public override bool Equals(object? obj)
    {
        return obj is ReservaCreateDTO dTO &&
               NombreCliente == dTO.NombreCliente &&
               ApellidosCliente == dTO.ApellidosCliente &&
               DireccionFacturacion == dTO.DireccionFacturacion &&
               MetodoPago == dTO.MetodoPago &&
               Impresora3DId == dTO.Impresora3DId &&
               TiempoReserva == dTO.TiempoReserva;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionFacturacion, MetodoPago, Impresora3DId, TiempoReserva);
    }
}