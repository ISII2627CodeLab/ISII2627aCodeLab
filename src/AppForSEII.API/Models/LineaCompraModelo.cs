namespace AppForSEII.API.Models
{
    public class LineaCompraModelo
    {
        public LineaCompraModelo()
        {
            Compra = null!;
            Modelo = null!;
        }

        public LineaCompraModelo(int cantidadLicencias, decimal precioUnidad, CompraModelo3D compra, Modelo3D modelo)
        {
            CantidadLicencias = cantidadLicencias;
            PrecioUnidad = precioUnidad;
            Subtotal = cantidadLicencias * precioUnidad;
            Compra = compra;
            CompraModelo3DId = compra.Id;
            Modelo = modelo;
            Modelo3DId = modelo.Id;
        }

        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int CantidadLicencias { get; set; }

        [Precision(10, 2)]
        public decimal PrecioUnidad { get; set; }

        [Precision(10, 2)]
        public decimal Subtotal { get; set; }

        // Relación N a 1 con la compra
        public int CompraModelo3DId { get; set; }

        [ForeignKey(nameof(CompraModelo3DId))]
        public CompraModelo3D Compra { get; set; }

        // Relación N a 1 con el modelo 3D
        public int Modelo3DId { get; set; }

        [ForeignKey(nameof(Modelo3DId))]
        public Modelo3D Modelo { get; set; }
    }
}
