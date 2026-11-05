using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.Modelo3DDTOs
{
    public class LineaCompraModelo3DCreateDTO
    {
        public LineaCompraModelo3DCreateDTO()
        {
        }

        public LineaCompraModelo3DCreateDTO(int modelo3DId, int cantidadLicencias)
        {
            Modelo3DId = modelo3DId;
            CantidadLicencias = cantidadLicencias;
        }

        [Required(ErrorMessage = "El modelo 3D es obligatorio")]
        public int Modelo3DId { get; set; }

        [Required(ErrorMessage = "La cantidad de licencias es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe comprar al menos 1 licencia")]
        public int CantidadLicencias { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is LineaCompraModelo3DCreateDTO otro)
            {
                return Modelo3DId == otro.Modelo3DId &&
                       CantidadLicencias == otro.CantidadLicencias;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modelo3DId, CantidadLicencias);
        }
    }
}