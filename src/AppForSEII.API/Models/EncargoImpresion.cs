
namespace AppForSEII.API.Models
{
    public class EncargoImpresion
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public DateTime FechaEncargo { get; set; }

        [Required]
        public string ApellidosCliente { get; set; }

        [Required]
        public string DireccionEnvio { get; set; }

        [Required]
        public string NumeroTelefono { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        // Relaciones
        public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();

        
    }
}