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
    public class InventarioActualRepository : Repository<InventarioActual>, IInventarioActualRepository
    {
        private readonly ApplicationDbContext _db;

        public InventarioActualRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(InventarioActual inventarioActual)
        {
            var objDesdeDb = _db.InventarioActual.FirstOrDefault(s => s.id_inventario == inventarioActual.id_inventario);

            if (objDesdeDb != null)
            {
                // Actualizar todas las propiedades
                objDesdeDb.id_producto = inventarioActual.id_producto;
                objDesdeDb.cantidad_disponible = inventarioActual.cantidad_disponible;
                objDesdeDb.stock_minimo = inventarioActual.stock_minimo;
                objDesdeDb.ultima_actualizacion = inventarioActual.ultima_actualizacion;

              
            }
        }
    }
}
