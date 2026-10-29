namespace   AppForSEII.API.DTOs.ApplicationUserDTOs.Impresora3DDTOs;

public class selectImpresora3DDTO
{ 
    public selectImpresora3DDTO(int id, string nombre, string modelo, TipoImpresora tipo, string descripcion, decimal precioKilovatioHora, decimal precioReserva)
    {
        Id = id;
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    }

    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; }

    [StringLength(50, ErrorMessage = "El modelo no puede superar los 50 caracteres.")]
    public string Modelo { get; set; }

    public TipoImpresora Tipo { get; set; }

    public string Descripcion { get; set; }

    public decimal PrecioKilovatioHora { get; set; }

    public decimal PrecioReserva { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is selectImpresora3DDTO dTO &&
               Id == dTO.Id &&
               Nombre == dTO.Nombre &&
               Modelo == dTO.Modelo &&
               Tipo == dTO.Tipo &&
               Descripcion == dTO.Descripcion &&
               PrecioKilovatioHora == dTO.PrecioKilovatioHora &&
               PrecioReserva == dTO.PrecioReserva;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Nombre, Modelo, Tipo, Descripcion, PrecioKilovatioHora, PrecioReserva);
    }
}