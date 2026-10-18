namespace AppForSEII.API.Models
{
    public class LineaCompraModelo
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int CantidadLicencias { get; set; }

        [Precision(10, 2)]
        public decimal PrecioUnidad { get; set; }

        [Precision(10, 2)]
        public decimal Subtotal { get; set; }

        // Relación con la compra modelo 3d
        public int CompraModelo3DId { get; set; }

        [ForeignKey(nameof(CompraModelo3DId))]
        public CompraModelo3D Compra { get; set; } = null!;

        // Relación con el modelo 3d
        public int Modelo3DId { get; set; }

        [ForeignKey(nameof(Modelo3DId))]
        public Modelo3D Modelo { get; set; } = null!;

        public LineaCompraModelo()
        {
        }

        public LineaCompraModelo(int cantidadLicencias, decimal precioUnidad, CompraModelo3D compra, Modelo3D modelo)
        {
            CantidadLicencias = cantidadLicencias;
            PrecioUnidad = precioUnidad;
            Subtotal = cantidadLicencias * precioUnidad;
            Compra = compra;
            Modelo = modelo;
        }
    }
}