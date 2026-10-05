namespace AppForSEII.API.Models
{
    public class ReservaImpresora
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        public DateTime FechaReserva { get; set; }

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string ApellidosCliente { get; set; } = string.Empty;

        [Required]
        public string DireccionFacturacion { get; set; } = string.Empty;

        [Required]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public string? ClienteId { get; set; }
       public Cliente? Cliente { get; set; }

        public IList<LineaReserva> LineaReservas { get; set; } = new List<LineaReserva>();
    }
}