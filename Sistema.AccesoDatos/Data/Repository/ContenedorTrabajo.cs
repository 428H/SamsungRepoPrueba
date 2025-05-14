using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository
{
    
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Producto = new ProductoRepository(_db);
            Cliente = new ClienteRepository(_db);
            TipoMovimientoI = new TipoMovimientoIRepository(_db);
            Usuario = new UsuarioRepository(_db);
            InventarioActual = new InventarioActualRepository(_db);
            MovimientoInventario = new MovimientoInventarioRepository(_db);
            MetodosPago = new MetodosPagoRepository(_db);
            HistorialPrecios = new HistorialPreciosRepository(_db);
            Venta = new VentaRepository(_db);
            DetalleVenta = new DetalleVentaRepository(_db);
            Factura = new FacturaRepository(_db);
        }

        public ICategoriaRepository Categoria { get; private set; }

        public IProductoRepository Producto  { get; private set; }

        public IClienteRepository Cliente { get; private set; }

        public ITipoMovimientoIRepository TipoMovimientoI { get; private set; }
        public IUsuarioRepository Usuario { get; private set; }
        public IInventarioActualRepository InventarioActual { get; private set; }
        public IMovimientoInventarioRepository MovimientoInventario { get; private set; }

        public IMetodosPagoRepository MetodosPago  { get; private set; }

        public IHistorialPreciosRepository HistorialPrecios { get; private set; }
        public IVentaRepository Venta { get; private set; }
        public IDetalleVentaRepository DetalleVenta { get; private set; }
        public IFacturaRepository Factura { get; private set; }

        public void Dispose()// metodo para liberar procesos en desuso
        {
            _db.Dispose();
        }
        public void save()
        {
            _db.SaveChanges();
        }
    }
}
