namespace AppForSEII.API.DTOs.Pieza3DDTOs
{
    public class Pieza3DSelectDTO
    {
        public Pieza3DSelectDTO(int id, string nombre, decimal precioEstimado)
        {
            Id = id;
            Nombre = nombre;
            PrecioEstimado = precioEstimado;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public decimal PrecioEstimado { get; set; }

          public override bool Equals(object? obj){
            if (obj is Pieza3DSelectDTO otro){
                return Id == otro.Id && Nombre == otro.Nombre && PrecioEstimado == otro.PrecioEstimado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PrecioEstimado);
        }
    }
}
