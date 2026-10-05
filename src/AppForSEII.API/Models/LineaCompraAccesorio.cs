namespace AppForSEII.API.Models;

public class LineaCompraAccesorio
{
    public LineaCompraAccesorio()
    {
    }

    public LineaCompraAccesorio(int id, int cantidad, decimal precioUnidad, decimal subtotal, Accesorio accesorio, CompraAccesorios? compraAccesorios = null)
    {
        Id = id;
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;

        if (accesorio != null)
        {
            Accesorio = accesorio;
            AccesorioId = accesorio.Id;
        }

        if (compraAccesorios != null)
        {
            CompraAccesorios = compraAccesorios;
            CompraAccesoriosId = compraAccesorios.Id;
        }
    }

    public int Id { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnidad { get; set; }

    public decimal Subtotal { get; set; }

    //Relacion con CompraAccesorios
    public int CompraAccesoriosId { get; set; }
    public CompraAccesorios CompraAccesorios { get; set; } = default!;

    //Relacion con Accesorio
    public int AccesorioId { get; set; }
    public Accesorio Accesorio { get; set; } = default!;
}