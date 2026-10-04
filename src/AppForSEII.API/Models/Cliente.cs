namespace AppForSEII.API.Models
{
    public class Cliente : ApplicationUser
    {
        public Cliente()
        {
        }
        public Cliente(string id, string name, string surname, string userName, string direccionFacturacion)
        : base(id, name, surname, userName)
        {
        DireccionFacturacion = direccionFacturacion;
        }
        [Required(ErrorMessage = "La dirección de facturación es obligatoria.")]
        [StringLength(200, MinimumLength = 1,
            ErrorMessage = "La dirección debe tener entre 1 y 200 caracteres.")]
        public string DireccionFacturacion { get; set; } = string.Empty;
    }
}