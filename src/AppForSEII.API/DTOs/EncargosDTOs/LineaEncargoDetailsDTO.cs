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
        
        public override bool Equals(object? obj)
        {
            if (obj is LineaEncargoDetailsDTO otro)
            {
                return NombrePieza == otro.NombrePieza &&
                    MaterialNombre == otro.MaterialNombre &&
                    Cantidad == otro.Cantidad &&
                    PrecioUnidad == otro.PrecioUnidad &&
                    Subtotal == otro.Subtotal &&
                    Descripcion == otro.Descripcion;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombrePieza, MaterialNombre, Cantidad, PrecioUnidad, Subtotal);
        }
    }
}