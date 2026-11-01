using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoCreateDTO
    {
        public EncargoCreateDTO()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            LineasEncargo = new List<LineaEncargoCreateDTO>();
        }

        public EncargoCreateDTO(string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, string? descripcion, MetodoPago metodoPago, List<LineaEncargoCreateDTO> lineasEncargo)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            Descripcion = descripcion;
            MetodoPago = metodoPago;
            LineasEncargo = lineasEncargo;
        }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string ApellidosCliente { get; set; }

        [Required(ErrorMessage = "La dirección de envío es obligatoria")]
        public string DireccionEnvio { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string NumeroTelefono { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe haber al menos una línea de encargo")]
        public List<LineaEncargoCreateDTO> LineasEncargo { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is EncargoCreateDTO otro)
            {
                return NombreCliente == otro.NombreCliente &&
                       ApellidosCliente == otro.ApellidosCliente &&
                       DireccionEnvio == otro.DireccionEnvio &&
                       NumeroTelefono == otro.NumeroTelefono &&
                       Descripcion == otro.Descripcion &&
                       MetodoPago == otro.MetodoPago &&
                       LineasEncargo.SequenceEqual(otro.LineasEncargo);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionEnvio, NumeroTelefono, MetodoPago);
        }
    }
}