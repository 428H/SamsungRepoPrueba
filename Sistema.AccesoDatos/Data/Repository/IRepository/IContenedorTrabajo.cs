using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo
    {
        ICategoriaRepository Categoria { get; }
        IProductoRepository Producto { get; }
        IClienteRepository Cliente { get; }
        ITipoMovimientoIRepository TipoMovimientoI { get; }
        IUsuarioRepository Usuario { get; }
        IInventarioActualRepository InventarioActual { get; }
        IMovimientoInventarioRepository MovimientoInventario { get; }
        IMetodosPagoRepository MetodosPago { get; }
        IHistorialPreciosRepository HistorialPrecios { get; }
        IVentaRepository Venta { get; }
        IDetalleVentaRepository DetalleVenta { get; }

        IFacturaRepository Factura { get; }
        void save();
    }
}
