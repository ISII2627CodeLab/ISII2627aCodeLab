using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class LineaEncargoCreateDTO
    {
        public LineaEncargoCreateDTO(){
         }

        public LineaEncargoCreateDTO(int cantidad, int pieza3DId, int materialId)
        {
            Cantidad = cantidad;
            Pieza3DId = pieza3DId;
            MaterialId = materialId;
        }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La pieza 3D es obligatoria")]
        public int Pieza3DId { get; set; }

        [Required(ErrorMessage = "El material es obligatorio")]
        public int MaterialId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is LineaEncargoCreateDTO otro)
            {
                return Cantidad == otro.Cantidad &&
                       Pieza3DId == otro.Pieza3DId &&
                       MaterialId == otro.MaterialId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cantidad, Pieza3DId, MaterialId);
        }
    }
}