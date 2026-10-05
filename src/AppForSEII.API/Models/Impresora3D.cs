namespace AppForSEII.API.Models;

public class Impresora3D
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required] 
    public string Modelo { get; set; } = string.Empty;

    [Required]
    public TipoImpresora Tipo { get; set; } 

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public decimal PrecioKilovatioHora { get; set; }

    public IList<LineaReserva> LineaReservas { get; set; } = new List<LineaReserva>();
}