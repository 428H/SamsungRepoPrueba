using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }
        public InventarioActual InventarioActual { get; set; }
        public MovimientoInventario MovimientoInventario { get; set; }
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
        public IEnumerable<SelectListItem> ListaTiposMovimiento { get; set; }

       
    }
}
