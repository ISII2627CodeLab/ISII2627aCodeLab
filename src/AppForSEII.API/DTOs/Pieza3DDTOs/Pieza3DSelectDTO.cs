namespace AppForSEII.API.DTOs.Pieza3DDTOs
{
    public class Pieza3DSelectDTO
    {
        public Pieza3DSelectDTO()
        {
            Nombre = string.Empty;
            Descripcion = string.Empty;
            MaterialNombre = string.Empty;
        }
        public Pieza3DSelectDTO(int id, string nombre, string descripcion, double pesoGramos, decimal precioEstimado, CategoriaPieza categoria, string materialNombre)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            PesoGramos = pesoGramos;
            PrecioEstimado = precioEstimado;
            Categoria = categoria;
            MaterialNombre = materialNombre;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double PesoGramos { get; set; }
        public decimal PrecioEstimado { get; set; }
        public CategoriaPieza Categoria { get; set; }
        public string MaterialNombre { get; set; }

          public override bool Equals(object? obj){
            if (obj is Pieza3DSelectDTO otro){
                return Id == otro.Id && Nombre == otro.Nombre && PrecioEstimado == otro.PrecioEstimado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Descripcion, PesoGramos, PrecioEstimado, Categoria, MaterialNombre);
        }
    }
}