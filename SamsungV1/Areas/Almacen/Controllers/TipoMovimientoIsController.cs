using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsungV1.Data;
using SamsungV1.Data.Migrations;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;

namespace SamsungV1.Areas.Almacen.Controllers
{
    [Area("Almacen")]
    [Authorize(Roles = "Admin,Almacen")]
    public class TipoMovimientoIsController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public TipoMovimientoIsController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoMovimientoI tipoMovimientoI)
        {

            if (tipoMovimientoI.estado != "activo" && tipoMovimientoI.estado != "inactivo")
            {
                ModelState.AddModelError("estado", "El estado debe ser 'activo' o 'inactivo'");
            }
            var TipoMovimientoExistente = _contenedorTrabajo.TipoMovimientoI.GetAll().FirstOrDefault(t =>
                t.nombre.ToLower() == tipoMovimientoI.nombre.ToLower());

            if (TipoMovimientoExistente != null)
            {
                ModelState.AddModelError("nombre", "Ya existe una tipo de movimineto con este nombre");
            }
            if (ModelState.IsValid)
            {

                _contenedorTrabajo.TipoMovimientoI.Add(tipoMovimientoI);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMovimientoI);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            TipoMovimientoI tipoMovimientoI = new TipoMovimientoI();
            tipoMovimientoI = _contenedorTrabajo.TipoMovimientoI.Get(id);
            if (tipoMovimientoI == null)
            {
                return NotFound();
            }
            return View(tipoMovimientoI);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TipoMovimientoI tipoMovimientoI)
        {
            var TipoMovimientoExistente = _contenedorTrabajo.TipoMovimientoI.GetAll().FirstOrDefault(t =>
               t.nombre.ToLower() == tipoMovimientoI.nombre.ToLower() &&
               t.id_tipomovimiento != tipoMovimientoI.id_tipomovimiento);

            if (TipoMovimientoExistente != null)
            {
                ModelState.AddModelError("nombre", "Ya existe otro tipo de movimineto con este nombre");
            }
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.TipoMovimientoI.Update(tipoMovimientoI);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMovimientoI);
        }

        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()//GETALL
        {
            return Json(new { data = _contenedorTrabajo.TipoMovimientoI.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.TipoMovimientoI.Get(id);



            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Tipo Movimiento" });
            }
            _contenedorTrabajo.TipoMovimientoI.Remove(objFromDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "Movimiento Borrada Exitosamente" });
        }
        #endregion
    }
}
