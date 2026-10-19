namespace AppForSEII.API.Models;

public class LineaEncargo
{
    public LineaEncargo()
    {
        EncargoImpresion = null!;
        Pieza = null!;
        MaterialSeleccionado = null!;
        EncargoImpresion = new EncargoImpresion();
        Pieza = new Pieza3D();
        MaterialSeleccionado = new Material();
    }
    public LineaEncargo(int cantidad, decimal precioUnidad, decimal subtotal)
    :this(){
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "La cantidad debe ser como mínimo 1 y como máximo 100.")]
    public int Cantidad { get; set; }

    [Required]
    [Precision(8, 2)]
    [Range(0.01, 999999.99, ErrorMessage = "El precio por unidad debe ser mayor que 0.")]
    public decimal PrecioUnidad { get; set; }

    [Required]
    [Precision(10, 2)]
    [Range(0.01, 99999999.99, ErrorMessage = "El subtotal debe ser mayor que 0.")]
    public decimal Subtotal { get; set; }

    //Relaciones
    public int EncargoImpresionId { get; set; }
    public EncargoImpresion EncargoImpresion { get; set; }

    public int Pieza3DId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Pieza3D Pieza { get; set; }

    public int MaterialId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Material MaterialSeleccionado { get; set; }
}