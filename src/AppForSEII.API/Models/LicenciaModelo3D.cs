using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class LicenciaModelo3D
    {
        public LicenciaModelo3D()
        {
            Nombre = string.Empty;
            Modelos3D = new List<Modelo3D>();
        }

        public LicenciaModelo3D(string nombre, DateTime fechaExpiracion) : this()
        {
            Nombre = nombre;
            FechaExpiracion = fechaExpiracion;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la licencia es obligatorio.")]
        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaExpiracion { get; set; }

        // Relación 1 a N con los modelos 3D
        public ICollection<Modelo3D> Modelos3D { get; set; }
    }
}
