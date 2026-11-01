namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class LineaEncargoDetailsDTO
    {
        public LineaEncargoDetailsDTO(string nombrePieza, string materialNombre, int cantidad, decimal precioUnidad, decimal subtotal, string? descripcion)
        {
            NombrePieza = nombrePieza;
            MaterialNombre = materialNombre;
            Cantidad = cantidad;
            PrecioUnidad = precioUnidad;
            Subtotal = subtotal;
            Descripcion = descripcion;
        }

        public string NombrePieza { get; set; }
        public string MaterialNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }
        public string? Descripcion { get; set; }
    }
}