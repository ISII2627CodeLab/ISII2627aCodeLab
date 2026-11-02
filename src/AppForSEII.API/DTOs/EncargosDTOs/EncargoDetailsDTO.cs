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

        public override bool Equals(object? obj)
        {
            if (obj is EncargoDetailsDTO otro)
            {
                return Id == otro.Id &&
                    // Comparamos la fecha truncando a fecha/minuto para evitar conflictos de milisegundos con DateTime.Now
                    FechaEncargo.Date == otro.FechaEncargo.Date &&
                    NombreCliente == otro.NombreCliente &&
                    ApellidosCliente == otro.ApellidosCliente &&
                    DireccionEnvio == otro.DireccionEnvio &&
                    NumeroTelefono == otro.NumeroTelefono &&
                    MetodoPago == otro.MetodoPago &&
                    Descripcion == otro.Descripcion &&
                    PrecioTotal == otro.PrecioTotal &&
                    LineasEncargo.SequenceEqual(otro.LineasEncargo);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombreCliente, ApellidosCliente, PrecioTotal);
        }
    }
}