using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.Impresora3DDTOs
{
    public class ReservaDetailDTO
    {
        public ReservaDetailDTO(int id, DateTime fechaReserva, string nombreCliente, string apellidosCliente, string direccionFacturacion, decimal precioTotal, MetodoPago metodoPago, List<LineaReservaDetailsDTO> lineasReserva)
        {
            Id = id;
            FechaReserva = fechaReserva;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionFacturacion = direccionFacturacion;
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            LineasReserva = lineasReserva; 
        }

        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string DireccionFacturacion { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<LineaReservaDetailsDTO> LineasReserva { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is ReservaDetailDTO otro)
            {
                return Id == otro.Id &&
                       FechaReserva.Date == otro.FechaReserva.Date &&
                       NombreCliente == otro.NombreCliente &&
                       ApellidosCliente == otro.ApellidosCliente &&
                       DireccionFacturacion == otro.DireccionFacturacion &&
                       MetodoPago == otro.MetodoPago &&
                       PrecioTotal == otro.PrecioTotal &&
                       LineasReserva.SequenceEqual(otro.LineasReserva);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombreCliente, ApellidosCliente, PrecioTotal);
        }
    }

    public class LineaReservaDetailsDTO
    {
        public LineaReservaDetailsDTO(string nombreImpresora, TiempoReserva tiempoReserva, decimal precioSubtotal)
        {
            NombreImpresora = nombreImpresora;
            TiempoReserva = tiempoReserva;
            PrecioSubtotal = precioSubtotal;
        }

        public string NombreImpresora { get; set; }
        public TiempoReserva TiempoReserva { get; set; }
        public decimal PrecioSubtotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LineaReservaDetailsDTO otro &&
                   NombreImpresora == otro.NombreImpresora &&
                   TiempoReserva == otro.TiempoReserva &&
                   PrecioSubtotal == otro.PrecioSubtotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreImpresora, TiempoReserva, PrecioSubtotal);
        }
    }
}