using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.Modelo3DDTOs
{
    public class LineaCompraModelo3DDetailDTO
    {
        public LineaCompraModelo3DDetailDTO()
        {
            
        }

        // para el POST: el cliente solo indica qué modelo y cuántas licencias
        public LineaCompraModelo3DDetailDTO(int modelo3DId, int cantidadLicencias)
        {
            Modelo3DId = modelo3DId;
            CantidadLicencias = cantidadLicencias;
        }
        public LineaCompraModelo3DDetailDTO(int modelo3DId, string nombreModelo, string categoria, FormatoModelo3D formato, decimal precioUnidad, int cantidadLicencias, decimal subtotal, DateTime fechaExpiracionLicencia)
        {
            Modelo3DId = modelo3DId;
            NombreModelo = nombreModelo;
            Categoria = categoria;
            Formato = formato;
            PrecioUnidad = precioUnidad;
            CantidadLicencias = cantidadLicencias;
            Subtotal = subtotal;
            FechaExpiracionLicencia = fechaExpiracionLicencia;
        }

        [Required(ErrorMessage = "El modelo 3D es obligatorio")]
        public int Modelo3DId { get; set; }

        [Required(ErrorMessage = "La cantidad de licencias es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe comprar al menos 1 licencia")]
        public int CantidadLicencias { get; set; }
        public string? NombreModelo { get; set; }
        public string? Categoria { get; set; }
        public FormatoModelo3D Formato { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime FechaExpiracionLicencia { get; set; }

        //Flujo alternativo 7.4: si la licencia caduca en el próximo mes, se avisa de la fecha límite
        public string? AvisoLicencia =>
            FechaExpiracionLicencia <= DateTime.Now.AddMonths(1)
                ? $"Atención: la licencia de este modelo caduca el {FechaExpiracionLicencia:dd/MM/yyyy}."
                : null;

        public override bool Equals(object? obj)
        {
            if (obj is LineaCompraModelo3DDetailDTO otro)
            {
                return Modelo3DId == otro.Modelo3DId &&
                       NombreModelo == otro.NombreModelo &&
                       Categoria == otro.Categoria &&
                       Formato == otro.Formato &&
                       PrecioUnidad == otro.PrecioUnidad &&
                       CantidadLicencias == otro.CantidadLicencias &&
                       Subtotal == otro.Subtotal &&
                       FechaExpiracionLicencia == otro.FechaExpiracionLicencia;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modelo3DId, NombreModelo, Categoria, Formato, PrecioUnidad, CantidadLicencias, Subtotal, FechaExpiracionLicencia);
        }
    }
}