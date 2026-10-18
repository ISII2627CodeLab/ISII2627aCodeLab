namespace AppForSEII.API.Models
{
    public class CompraModelo3D
    {
        public CompraModelo3D()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            CorreoElectronico = string.Empty;
            DireccionFacturacion = string.Empty;
            ClienteId = string.Empty;
            Cliente = null!;
            LineasCompra = new List<LineaCompraModelo>();
        }

        public CompraModelo3D(DateTime fechaCompra, string nombreCliente, string apellidosCliente,
                              string correoElectronico, string direccionFacturacion,
                              string? descripcion, decimal precioTotal,
                              MetodoPago metodoPago, Cliente cliente) : this()
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
            ClienteId = cliente.Id;
        }

        [Key]
        public int Id { get; set; }

        public DateTime FechaCompra { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string ApellidosCliente { get; set; }

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        public string DireccionFacturacion { get; set; }

        public string? Descripcion { get; set; }

        [Precision(10, 2)]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        // Relación N a 1 con el cliente
        [Required]
        public string ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Cliente Cliente { get; set; }

        // Relación 1 a N con las líneas de compra
        public ICollection<LineaCompraModelo> LineasCompra { get; set; }
    }
}
