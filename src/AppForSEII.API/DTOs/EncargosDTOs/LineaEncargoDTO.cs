using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class LineaEncargoDTO
    {
        public LineaEncargoDTO()
        {
            NombrePieza = string.Empty;
            MaterialNombre = string.Empty;
        }

        public LineaEncargoDTO(int cantidad, int pieza3DId, int materialId)
        {
            Cantidad = cantidad;
            Pieza3DId = pieza3DId;
            MaterialId = materialId;
            NombrePieza = string.Empty;
            MaterialNombre = string.Empty;
        }

        public LineaEncargoDTO(string nombrePieza, string materialNombre, int cantidad, decimal precioUnidad, decimal subtotal, string? descripcion, int pieza3DId = 0, int materialId = 0)
        {
            NombrePieza = nombrePieza;
            MaterialNombre = materialNombre;
            Cantidad = cantidad;
            PrecioUnidad = precioUnidad;
            Subtotal = subtotal;
            Descripcion = descripcion;
            Pieza3DId = pieza3DId;
            MaterialId = materialId;
        }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Cantidad { get; set; }

        public int Pieza3DId { get; set; }
        public int MaterialId { get; set; }

        public string NombrePieza { get; set; }
        public string MaterialNombre { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }
        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is LineaEncargoDTO otro)
            {
                return Cantidad == otro.Cantidad &&
                       Pieza3DId == otro.Pieza3DId &&
                       MaterialId == otro.MaterialId &&
                       NombrePieza == otro.NombrePieza &&
                       MaterialNombre == otro.MaterialNombre &&
                       PrecioUnidad == otro.PrecioUnidad &&
                       Subtotal == otro.Subtotal &&
                       Descripcion == otro.Descripcion;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cantidad, Pieza3DId, MaterialId, NombrePieza, Subtotal);
        }
    }
}