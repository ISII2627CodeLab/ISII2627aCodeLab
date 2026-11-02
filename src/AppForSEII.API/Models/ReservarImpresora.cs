namespace AppForSEII.API.Models
{
    public class ReservaImpresora
    {
        public ReservaImpresora()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionFacturacion = string.Empty;
            LineaReservas = new List<LineaReserva>();

        }
        public ReservaImpresora(string nombreCliente, string apellidosCliente, string direccionFacturacion, decimal precioTotal, MetodoPago metodoPago
        )
        :this(){
            FechaReserva = DateTime.Now;
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionFacturacion = string.Empty;
            PrecioTotal = 0;
            MetodoPago = MetodoPago.Bizum;
            
        }
        [Key]
        public int Id { get; set; }

        [Required]  
        public DateTime FechaReserva { get; set; }

        [Required]
        public string NombreCliente { get; set; } 

        [Required]
        public string ApellidosCliente { get; set; } 

        [Required]
        public string DireccionFacturacion { get; set; } 

        [Required]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public string? ClienteId { get; set; }
       public Cliente? Cliente { get; set; }

        public IList<LineaReserva> LineaReservas { get; set; } 
    }
}