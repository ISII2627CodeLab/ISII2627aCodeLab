using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Accesorio
{
    public Accesorio()
    {
    }

    public Accesorio(int id, string nombre, CategoriaAccesorio categoria, string compatibilidad, int cantidadDisponible, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Compatibilidad = compatibilidad;
        CantidadDisponible = cantidadDisponible;
        Precio = precio;
    }

    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public CategoriaAccesorio Categoria { get; set; }

    [StringLength(200)]
    public string Compatibilidad { get; set; } = string.Empty;

    public int CantidadDisponible { get; set; }

    public decimal Precio { get; set; }

    public ICollection<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();
}