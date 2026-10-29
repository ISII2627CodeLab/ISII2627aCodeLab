using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

public class Material
{
    public Material()
    {
        Nombre = string.Empty;
        PiezasMaterial = new List<PiezaMaterial>();
        LineasEncargo = new List<LineaEncargo>();
    }

    public Material(string nombre, decimal precioPorGramo, decimal stockGramos)
        : this()
    {
        Nombre = nombre;
        PrecioPorGramo = precioPorGramo;
        StockGramos = stockGramos;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }

    [Required]
    [Range(0.01, 1000.00)]
    [Precision(18, 2)]
    public decimal PrecioPorGramo { get; set; }

    [Required]
    [Range(0.0, 100000.0)]
    [Precision(18, 2)]
    public decimal StockGramos { get; set; }

    // Relaciones
    public ICollection<PiezaMaterial> PiezasMaterial { get; set; }
    public ICollection<LineaEncargo> LineasEncargo { get; set; }



}