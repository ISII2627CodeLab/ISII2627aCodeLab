namespace AppForSEII.API.Models
{
    public class CompraModelo3D
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaCompra { get; set; }

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string ApellidosCliente { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string DireccionFacturacion { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Precision(10, 2)]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public string ClienteId { get; set; } = string.Empty;

        [ForeignKey(nameof(ClienteId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Cliente Cliente { get; set; } = null!;

        public CompraModelo3D()
        {
        }

        public CompraModelo3D(DateTime fechaCompra, string nombreCliente, string apellidosCliente,
                              string correoElectronico, string direccionFacturacion,
                              string? descripcion, decimal precioTotal,
                              MetodoPago metodoPago, Cliente cliente)
        {
            FechaCompra = fechaCompra;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            CorreoElectronico = correoElectronico;
            DireccionFacturacion = direccionFacturacion;
            Descripcion = descripcion;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            Cliente = cliente;
        }
    }
}