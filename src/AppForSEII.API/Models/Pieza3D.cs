namespace AppForSEII.API.Models;

public class Pieza3D
{

    public Pieza3D()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        LineasEncargo = new List<LineaEncargo>();
        PiezaMaterials = new List<PiezaMaterial>();
    }

    public Pieza3D(string nombre, string descripcion, double pesoGramos, int cantidad, CategoriaPieza categoria)
    : this(){
        Nombre = nombre;
        Descripcion = descripcion;
        PesoGramos = pesoGramos;
        Cantidad = cantidad;
        Categoria = categoria;
        
        
    }

    
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(25, ErrorMessage = "El nombre no puede superar los 25 caracteres.", MinimumLength = 2)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "La descripcion no puede tener más de 100 caracteres.")]
    public string Descripcion { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "El peso debe estar entre 1 y 1000 gramos.")]
    public double PesoGramos { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "La cantidad debe ser mayor o igual a 1.")]
    public int Cantidad { get; set; }
    public CategoriaPieza Categoria { get; set; }

    // Relaciones
    public IList<LineaEncargo> LineasEncargo { get; set; }
    public ICollection<PiezaMaterial> PiezaMaterials { get; set; }
}