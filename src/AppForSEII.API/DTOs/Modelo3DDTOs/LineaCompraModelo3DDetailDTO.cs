using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.Modelo3DDTOs
{
    public class LineaCompraModelo3DDetailDTO
    {
        public LineaCompraModelo3DDetailDTO(string nombreModelo, string categoria, FormatoModelo3D formato, decimal precioUnidad, int cantidadLicencias, decimal subtotal, DateTime fechaExpiracionLicencia)
        {
            NombreModelo = nombreModelo;
            Categoria = categoria;
            Formato = formato;
            PrecioUnidad = precioUnidad;
            CantidadLicencias = cantidadLicencias;
            Subtotal = subtotal;
            FechaExpiracionLicencia = fechaExpiracionLicencia;
        }

        public string NombreModelo { get; set; }
        public string Categoria { get; set; }
        public FormatoModelo3D Formato { get; set; }
        public decimal PrecioUnidad { get; set; }
        public int CantidadLicencias { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime FechaExpiracionLicencia { get; set; }

        // Flujo alternativo 7.4: si la licencia caduca en el próximo mes, se avisa de la fecha límite
        public string? AvisoLicencia =>
            FechaExpiracionLicencia <= DateTime.Now.AddMonths(1)
                ? $"Atención: la licencia de este modelo caduca el {FechaExpiracionLicencia:dd/MM/yyyy}."
                : null;

        public override bool Equals(object? obj)
        {
            if (obj is LineaCompraModelo3DDetailDTO otro)
            {
                return NombreModelo == otro.NombreModelo &&
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
            return HashCode.Combine(NombreModelo, Categoria, Formato, PrecioUnidad, CantidadLicencias, Subtotal, FechaExpiracionLicencia);
        }
    }
}