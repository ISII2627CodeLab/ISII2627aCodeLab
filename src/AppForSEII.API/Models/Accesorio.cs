using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    public class Accesorio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public CategoriaAccesorio Categoria { get; set; }

        [Required]
        [StringLength(200)]
        public string Compatibilidad { get; set; } = string.Empty;

        [Required]
        public int CantidadDisponible { get; set; }

        [Precision(10, 2)]
        public decimal Precio { get; set; }

        public ICollection<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();

        public Accesorio()
        {
        }

        public Accesorio(string nombre, CategoriaAccesorio categoria, string compatibilidad, int cantidadDisponible, decimal precio)
        {
            Nombre = nombre;
            Categoria = categoria;
            Compatibilidad = compatibilidad;
            CantidadDisponible = cantidadDisponible;
            Precio = precio;
        }
    }
}
