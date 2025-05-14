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
    class VentaRepository : Repository<Venta>, IVentaRepository
    {

        private readonly ApplicationDbContext _db;

        public VentaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Venta venta)
        {
            var objDesdeDb = _db.Ventas.FirstOrDefault(s => s.Id_venta == venta.Id_venta);
            if (objDesdeDb != null)
            {
                objDesdeDb.Id_cliente = venta.Id_cliente;
                objDesdeDb.Subtotal = venta.Subtotal;
                objDesdeDb.Total = venta.Total;
                objDesdeDb.Id_metodo_pago = venta.Id_metodo_pago;
                objDesdeDb.Fecha = venta.Fecha;
                objDesdeDb.Id_usuario = venta.Id_usuario;
                objDesdeDb.Notas = venta.Notas;
            }
        }
        //public IEnumerable<SelectListItem> GetVentasDropdown()
        //{
        //    return _db.Ventas.Select(i => new SelectListItem()
        //    {
        //        Text = "Venta #" + i.Id_venta + " - " + i.Fecha.ToString("dd/MM/yyyy"),
        //        Value = i.Id_venta.ToString()
        //    });
        //}
    }
}
