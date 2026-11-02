namespace AppForSEII.API.Models;

public class Impresora3D
{
    public Impresora3D()
    {
        Nombre = string.Empty;
        Modelo = string.Empty;
        Descripcion = string.Empty;
        LineaReservas = new List<LineaReserva>();

        
    }
    public Impresora3D(string nombre, string modelo, TipoImpresora tipo, string descripcion, decimal precioKilovatioHora, decimal precioReserva)
    : this(){
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    } 
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } 

    [Required] 
    public string Modelo { get; set; } 

    [Required]
    public TipoImpresora Tipo { get; set; } 

    [Required]
    public string Descripcion { get; set; } 

    [Required]
    public decimal PrecioKilovatioHora { get; set; }

    [Required]
    public decimal PrecioReserva { get; set; }
    public IList<LineaReserva> LineaReservas { get; set; }
    
}