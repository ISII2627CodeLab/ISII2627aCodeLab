using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class LicenciaModelo3D
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la licencia es obligatorio.")]
        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public DateTime FechaExpiracion { get; set; }

        // Constructor vacío
        public LicenciaModelo3D()
        {
        }

        public LicenciaModelo3D(string nombre, DateTime fechaExpiracion)
        {
            Nombre = nombre;
            FechaExpiracion = fechaExpiracion;
        }
    }
}