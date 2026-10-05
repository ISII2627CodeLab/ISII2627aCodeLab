namespace AppForSEII.API.Models
{
    public class Modelo3D
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio.")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "La categoría debe tener entre 1 y 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        public FormatoModelo3D Formato { get; set; }

        [Required]
        [Precision(10, 2)]
        [Range(0.01, 10000000, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        // Relación N a 1 con la licencia
        [Required]
        public int LicenciaModelo3DId { get; set; }

        [ForeignKey(nameof(LicenciaModelo3DId))]
        public LicenciaModelo3D Licencia { get; set; } = null!;

        public Modelo3D()
        {
        }

        public Modelo3D(string nombre, string categoria, FormatoModelo3D formato,
                        decimal precio, LicenciaModelo3D licencia)
        {
            Nombre = nombre;
            Categoria = categoria;
            Formato = formato;
            Precio = precio;
            Licencia = licencia;
        }
    }
}