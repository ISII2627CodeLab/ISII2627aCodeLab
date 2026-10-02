namespace AppForSEII.API.Models;

public class Pieza3D
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(25, ErrorMessage = "El nombre no puede superar los 25 caracteres.", MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "La descripcion no puede tener más de 100 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000, ErrorMessage = "El peso debe estar entre 1 y 1000 gramos.")]
    public double PesoGramos { get; set; }

    [Required]
    [Precision(5, 2)]
    [Range(0.01, 999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal PrecioEstimado { get; set; }

    // Relaciones
    // public int MaterialId { get; set; }
    // public Material? Material { get; set; }
    // public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}