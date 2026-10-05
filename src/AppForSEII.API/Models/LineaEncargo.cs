namespace AppForSEII.API.Models;

public class LineaEncargo
{
    public LineaEncargo()
    {
    }
    public LineaEncargo(int cantidad, decimal precioUnidad, decimal subtotal)
    {
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
    public EncargoImpresion EncargoImpresion { get; set; } = null!;

    public int Pieza3DId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Pieza3D Pieza { get; set; } = null!;

    public int MaterialId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Material MaterialSeleccionado { get; set; } = null!;
}