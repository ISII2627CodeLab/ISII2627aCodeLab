namespace AppForSEII.API.DTOs.Modelo3DDTOs;

public class Modelo3DParaCompraDTO
{
    public Modelo3DParaCompraDTO(int id, string nombre, string categoria, FormatoModelo3D formato, decimal precio, string nombreLicencia, DateTime fechaExpiracionLicencia)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Formato = formato;
        Precio = precio;
        NombreLicencia = nombreLicencia;
        FechaExpiracionLicencia = fechaExpiracionLicencia;
    }

    public int Id { get; set; }
    [StringLength(100)]
    public string Nombre { get; set; }
    [StringLength(50)]
    public string Categoria { get; set; }
    public FormatoModelo3D Formato { get; set; }
    [Precision(10, 2)]
    public decimal Precio { get; set; }
    [StringLength(50)]
    public string NombreLicencia { get; set; }
    public DateTime FechaExpiracionLicencia { get; set; }
    public override bool Equals(object? obj)
    {
        return obj is Modelo3DParaCompraDTO dTO &&
               Id == dTO.Id &&
               Nombre == dTO.Nombre &&
               Categoria == dTO.Categoria &&
               Formato == dTO.Formato &&
               Precio == dTO.Precio &&
               NombreLicencia == dTO.NombreLicencia &&
               FechaExpiracionLicencia == dTO.FechaExpiracionLicencia;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Nombre, Categoria, Formato, Precio, NombreLicencia , FechaExpiracionLicencia);
    }
}
