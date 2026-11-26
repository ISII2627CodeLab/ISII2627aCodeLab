using AppForSEII.Web.API;

namespace AppForSEII.Web
{
    public class EncargoStateContainer
    {
        public EncargoCreateDTO Encargo { get; private set; } = new()
        {
            LineasEncargo = new List<LineaEncargoDTO>()
        };

        public void AddPiezaToEncargo(Pieza3DSelectDTO pieza)
        {
            //añade sin duplicar usando i d de pieza
            if (!Encargo.LineasEncargo.Any(li => li.Pieza3DId == pieza.Id)){
                Encargo.LineasEncargo.Add(new LineaEncargoDTO()
                {
                    Pieza3DId = pieza.Id,
                    NombrePieza = pieza.Nombre,
                    Cantidad = 1
                });
                ComputeTotals();
            }
        }

        public void RemovePiezaFromEncargo(LineaEncargoDTO linea)
        {
            Encargo.LineasEncargo.Remove(linea);
            ComputeTotals();
        }

        private void ComputeTotals()
        {
            //subtotal de cada linea
            foreach (var linea in Encargo.LineasEncargo)
            {
                linea.Subtotal = linea.Cantidad * linea.PrecioUnidad;
            }
        }
        public void ClearEncargo()
        {
            Encargo.LineasEncargo.Clear();
            ComputeTotals();
        }
    }
}