namespace AppForSEII.API.Models;

public class LineaReserva
{
    public LineaReserva()
    {
    }
    public LineaReserva(TiempoReserva tiempoReserva, decimal precioSubtotal, int impresora3DId)
    :this(){
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        Impresora3DId = impresora3DId;
    }
     
    [Key]
    public int Id { get; set; }

    [Required]
    public TiempoReserva TiempoReserva { get; set; }

    [Required]
    public decimal PrecioSubtotal { get; set; }

    [Required] 
   public int Impresora3DId { get; set; }
   public Impresora3D? Impresora3D { get; set; }
}