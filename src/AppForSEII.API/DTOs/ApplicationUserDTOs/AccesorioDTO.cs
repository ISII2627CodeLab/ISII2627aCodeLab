using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AccesorioDTOs
{
    public class AccesorioDTO
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public CategoriaAccesorio Categoria { get; set; }

        [StringLength(200)]
        public string Compatibilidad { get; set; } = string.Empty;

        public int CantidadDisponible { get; set; }

        public decimal Precio { get; set; }

        public AccesorioDTO()
        {
        }

        public AccesorioDTO(int id, string nombre, CategoriaAccesorio categoria, string compatibilidad, int cantidadDisponible, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Compatibilidad = compatibilidad;
            CantidadDisponible = cantidadDisponible;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is AccesorioDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria &&
                   Compatibilidad == dTO.Compatibilidad &&
                   CantidadDisponible == dTO.CantidadDisponible &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Categoria, Compatibilidad, CantidadDisponible, Precio);
        }
    }
}