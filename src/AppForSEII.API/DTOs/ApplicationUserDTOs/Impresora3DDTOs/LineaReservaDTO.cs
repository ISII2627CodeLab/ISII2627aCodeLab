using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ApplicationUserDTOs.Impresoras3DDTOs
{
    public class LineaReservaDTO
    {
        public LineaReservaDTO(int impresora3DId, TiempoReserva tiempoReserva)
        {
            Impresora3DId = impresora3DId;
            TiempoReserva = tiempoReserva;
        }

        public int Impresora3DId { get; set; }
        public TiempoReserva TiempoReserva { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LineaReservaDTO dto &&
                   Impresora3DId == dto.Impresora3DId &&
                   TiempoReserva == dto.TiempoReserva;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Impresora3DId, TiempoReserva);
        }
    }
} 