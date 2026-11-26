using AppForSEII.Web.API;

namespace AppForSEII.Web {
    public class CompraModelo3DStateContainer {
        public CompraModelo3DCreateDTO Compra { get; private set; } = new CompraModelo3DCreateDTO() {
            LineasCompra = new List<LineaCompraModelo3DDetailDTO>()
        };

        public double PrecioTotal { get; private set; }

        public void AddModelo3DToCompra(Modelo3DParaCompraDTO modelo) {
            if (!Compra.LineasCompra.Any(l => l.Modelo3DId == modelo.Id))
                Compra.LineasCompra.Add(new LineaCompraModelo3DDetailDTO() {
                    Modelo3DId = modelo.Id,
                    NombreModelo = modelo.Nombre,
                    Categoria = modelo.Categoria,
                    Formato = modelo.Formato,
                    PrecioUnidad = modelo.Precio,
                    CantidadLicencias = 1,
                    FechaExpiracionLicencia = modelo.FechaExpiracionLicencia
                });
            ComputeTotalPrice();
        }

        private void ComputeTotalPrice() {
            PrecioTotal = Compra.LineasCompra.Sum(l => l.PrecioUnidad * l.CantidadLicencias);
        }

        public void RemoveLineaCompra(LineaCompraModelo3DDetailDTO linea) {
            Compra.LineasCompra.Remove(linea);
            ComputeTotalPrice();
        }

        public void ClearCarrito() {
            Compra.LineasCompra.Clear();
            PrecioTotal = 0;
        }

        public void CompraProcessed() {
            Compra = new CompraModelo3DCreateDTO() {
                LineasCompra = new List<LineaCompraModelo3DDetailDTO>()
            };
            PrecioTotal = 0;
        }
    }
}