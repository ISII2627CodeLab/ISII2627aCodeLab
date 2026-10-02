namespace AppForSEII.API.Models;

public class Pieza3D
{
    public Pieza3D()
    {
        LineasEncargo = new List<LineaEncargo>();
    }

    public Pieza3D(string nombre, string? descripcion, decimal precioUnidad, double pesoGramos, int tiempoImpresionMin, CategoriaPieza categoriaPieza)
        : this()
    {
        Nombre = nombre;
        Descripcion = descripcion;
        PrecioUnidad = precioUnidad;
        PesoGramos = pesoGramos;
        TiempoImpresionMin = tiempoImpresionMin;
        CategoriaPieza = categoriaPieza;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; }

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, 10000.00)]
    [Precision(18, 2)]
    public decimal PrecioUnidad { get; set; }

    [Required]
    [Range(0.1, 50000.0)]
    public double PesoGramos { get; set; }

    [Required]
    [Range(1, 10080)]
    public int TiempoImpresionMin { get; set; }

    [Required]
    public CategoriaPieza CategoriaPieza { get; set; }

    public ICollection<LineaEncargo> LineasEncargo { get; set; }
}