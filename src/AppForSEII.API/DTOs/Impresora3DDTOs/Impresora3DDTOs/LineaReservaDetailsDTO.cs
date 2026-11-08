using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs
{
    public class LineaReservaDetailsDTO
    {
        public LineaReservaDetailsDTO(string nombreImpresora, string modelo, string tipo, string descripcion, TiempoReserva tiempoReserva, decimal precioKWh, decimal precioReserva, decimal precioSubtotal)
        {
            NombreImpresora = nombreImpresora;
            Modelo = modelo;
            Tipo = tipo;
            Descripcion = descripcion;
            TiempoReserva = tiempoReserva;
            PrecioKWh = precioKWh;
            PrecioReserva = precioReserva;
            PrecioSubtotal = precioSubtotal;
        }

        public string NombreImpresora { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public TiempoReserva TiempoReserva { get; set; }
        public decimal PrecioKWh { get; set; }
        public decimal PrecioReserva { get; set; }
        public decimal PrecioSubtotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LineaReservaDetailsDTO dto &&
                   NombreImpresora == dto.NombreImpresora &&
                   Modelo == dto.Modelo &&
                   Tipo == dto.Tipo &&
                   Descripcion == dto.Descripcion &&
                   TiempoReserva == dto.TiempoReserva &&
                   PrecioKWh == dto.PrecioKWh &&
                   PrecioReserva == dto.PrecioReserva &&
                   PrecioSubtotal == dto.PrecioSubtotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreImpresora, Modelo, Tipo, Descripcion, TiempoReserva, PrecioKWh, PrecioReserva);
        }
    }
} 