using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentaModel = Sistema.Models.Venta;

namespace Sistema.Models.ViewModels
{
    public class VentaVM
    {
        public VentaModel Venta { get; set; }
        public IEnumerable<SelectListItem> ClientesLista { get; set; }
        public IEnumerable<SelectListItem> MetodosPagoLista { get; set; }
        public IEnumerable<SelectListItem> ListaTiposMovimiento { get; set; }
        public List<ProductoSeleccionado> ProductosSeleccionados { get; set; } = new List<ProductoSeleccionado>();
        public IEnumerable<Cliente> Clientes { get; set; }
        public IEnumerable<Producto> Productos { get; set; }
        public int IdTipoMovimientoSeleccionado { get; set; }

    }
    public class ProductoSeleccionado
    {
        public int Id { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Nombre { get; set; } 
    }
}
