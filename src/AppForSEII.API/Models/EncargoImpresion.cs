
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class EncargoImpresion
    {
        public EncargoImpresion()
        {
            
        }

        public EncargoImpresion(DateTime fechaEncargo, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, string? descripcion, decimal precioTotal, MetodoPago metodoPago)
        {
            FechaEncargo = fechaEncargo;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            Descripcion = descripcion;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
        }

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
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        // Relaciones
        public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
    }
}