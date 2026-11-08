using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs
{
    public class ReservaDTO
    {
        public ReservaDTO(string nombreCliente, string apellidosCliente, string direccionFacturacion, MetodoPago metodoPago, List<LineaReservaDTO> lineasReserva)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionFacturacion = direccionFacturacion;
            MetodoPago = metodoPago;
            LineasReserva = lineasReserva;
        }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string ApellidosCliente { get; set; }

        [Required]
        public string DireccionFacturacion { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public List<LineaReservaDTO> LineasReserva { get; set; }
    }
} 