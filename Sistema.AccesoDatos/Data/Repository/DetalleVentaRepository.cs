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
    public class DetalleVentaRepository : Repository<DetalleVenta>, IDetalleVentaRepository
    {
        private readonly ApplicationDbContext _db;

        public DetalleVentaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DetalleVenta detalleVenta)
        {
            var objDesdeDb = _db.DetallesVenta.FirstOrDefault(s => s.Id_detalle == detalleVenta.Id_detalle);
            if (objDesdeDb != null)
            {
                objDesdeDb.Id_venta = detalleVenta.Id_venta;
                objDesdeDb.Id_producto = detalleVenta.Id_producto;
                objDesdeDb.Cantidad = detalleVenta.Cantidad;
                objDesdeDb.Precio_unitario = detalleVenta.Precio_unitario;
            }
        }
    }
}
