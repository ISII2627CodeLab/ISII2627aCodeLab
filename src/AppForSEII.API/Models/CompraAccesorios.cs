public class CompraAccesorios
{
    public int Id { get; set; }
    public DateTime FechaCompra { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string ApellidoCliente { get; set; } = string.Empty;
    public string DireccionEnvio { get; set; } = string.Empty;
    public decimal PrecioTotal { get; set; }
    public MetodoPago MetodoPago { get; set; } = default!;
}
