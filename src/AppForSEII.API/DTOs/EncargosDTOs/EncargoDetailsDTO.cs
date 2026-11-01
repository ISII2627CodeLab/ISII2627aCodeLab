using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoDetailsDTO
    {
        public EncargoDetailsDTO(int id, DateTime fechaEncargo, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, MetodoPago metodoPago, string? descripcion, decimal precioTotal, List<LineaEncargoDetailsDTO> lineasEncargo)
        {
            Id = id;
            FechaEncargo = fechaEncargo;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            MetodoPago = metodoPago;
            Descripcion = descripcion;
            PrecioTotal = precioTotal;
            LineasEncargo = lineasEncargo;
        }

        public int Id { get; set; }
        public DateTime FechaEncargo { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string DireccionEnvio { get; set; }
        public string NumeroTelefono { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<LineaEncargoDetailsDTO> LineasEncargo { get; set; }
    }
}