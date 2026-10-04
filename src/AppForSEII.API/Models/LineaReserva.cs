namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    //[Required]
    //public TiempoReserva TiempoReserva { get; set; }

    [Required]
    public decimal PrecioSubtotal { get; set; }

   // [Required]
   // public int Impresora3DId { get; set; }
   // public Impresora3D? Impresora3D { get; set; }
}