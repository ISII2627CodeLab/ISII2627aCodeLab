using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    public class CompraAccesorios
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaCompra { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ApellidosCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NumeroTelefono { get; set; } = string.Empty;

        [Precision(10, 2)]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public string? ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Cliente? Cliente { get; set; }

        public ICollection<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();

        public CompraAccesorios()
        {
        }

        public CompraAccesorios(DateTime fechaCompra, string nombreCliente, string apellidosCliente,
                                string direccionEnvio, string numeroTelefono, decimal precioTotal,
                                MetodoPago metodoPago, Cliente? cliente = null)
        {
            FechaCompra = fechaCompra;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;

            if (cliente != null)
            {
                Cliente = cliente;
                ClienteId = cliente.Id;
            }
        }
    }
}
