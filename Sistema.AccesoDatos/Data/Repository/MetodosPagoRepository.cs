using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class MetodosPagoRepository : Repository<MetodosPago>, IMetodosPagoRepository
    {
        private readonly ApplicationDbContext _db;

        public MetodosPagoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       

        public void Update(MetodosPago metodosPago)
        {
            var objDesdeDb = _db.MetodosPagos.FirstOrDefault(s => s.Id_metodoPago == metodosPago.Id_metodoPago);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = metodosPago.Nombre;
                objDesdeDb.Descripcion = metodosPago.Descripcion;
                objDesdeDb.Estado = metodosPago.Estado;

                _db.SaveChanges();
            }
        }
        public IEnumerable<SelectListItem> GetAllMetodosPago()
        {
            return _db.MetodosPagos.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id_metodoPago.ToString()
            });
        }
    }
}
