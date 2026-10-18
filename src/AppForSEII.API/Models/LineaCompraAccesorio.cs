using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    public class LineaCompraAccesorio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Precision(10, 2)]
        public decimal PrecioUnidad { get; set; }

        [Precision(10, 2)]
        public decimal Subtotal { get; set; }

        [Required]
        public int CompraAccesoriosId { get; set; }

        [ForeignKey(nameof(CompraAccesoriosId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public CompraAccesorios CompraAccesorios { get; set; } = null!;

        [Required]
        public int AccesorioId { get; set; }

        [ForeignKey(nameof(AccesorioId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Accesorio Accesorio { get; set; } = null!;

        public LineaCompraAccesorio()
        {
        }

        public LineaCompraAccesorio(int cantidad, decimal precioUnidad, decimal subtotal,
                                    Accesorio accesorio, CompraAccesorios? compraAccesorios = null)
        {
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
    }
}
