public class LineaCompraAccesorio{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitarioAplicado { get; set; }
    public int CompraAccesoriosId { get; set; }
    public CompraAccesorios CompraAccesorios { get; set; } = default!;
    public int AccesorioId { get; set; }
    public Accesorio Accesorio { get; set; } = default!;
    public decimal Subtotal => Cantidad * PrecioUnitarioAplicado;
}