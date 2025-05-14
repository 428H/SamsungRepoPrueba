using Microsoft.AspNetCore.Mvc.Rendering;
using SamsungV1.Data;
using SamsungV1.Data.Migrations;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository
{
    public class TipoMovimientoIRepository: Repository<TipoMovimientoI> ,ITipoMovimientoIRepository
    {
        private readonly ApplicationDbContext _db;
        public TipoMovimientoIRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }

        public IEnumerable<SelectListItem> GetListaTiposMovimiento()
        {
            return _db.tipoMovimientoI.Where(i => i.estado == "activo")
                .Select(i => new SelectListItem()
                {
                    Text = i.nombre,
                    Value = i.id_tipomovimiento.ToString()
                });
        }

        public void Update(TipoMovimientoI tipoMovimientoI)
        {

            var objDesdeDb = _db.tipoMovimientoI.FirstOrDefault(s => s.id_tipomovimiento == tipoMovimientoI.id_tipomovimiento);
            if (objDesdeDb != null)
            {
                objDesdeDb.nombre = tipoMovimientoI.nombre;
                objDesdeDb.afecta_stock = tipoMovimientoI.afecta_stock;
                objDesdeDb.descripcion = tipoMovimientoI.descripcion;
                objDesdeDb.estado = tipoMovimientoI.estado;

            }
            else
            {

                throw new Exception($"No se encontró: {tipoMovimientoI.id_tipomovimiento}");
            }

            _db.SaveChanges();
        }
    }
}
