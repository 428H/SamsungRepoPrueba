using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;

namespace SamsungV1.Areas.Venta.Controllers
{

    [Area("Venta")]
    [Authorize(Roles = "Admin, Venta")]
    public class MetodosPagoController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;
        public MetodosPagoController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MetodosPago metodosPago)
        {

            if (metodosPago.Estado != "Activo" && metodosPago.Estado != "Inactivo")
            {
                ModelState.AddModelError("estado", "El estado debe ser 'activo' o 'inactivo'");
            }
            var metodoPagoExistente = _contenedorTrabajo.MetodosPago.GetAll().FirstOrDefault(m =>
                m.Nombre.ToLower() == metodosPago.Nombre.ToLower());

            if (metodoPagoExistente != null)
            {
                ModelState.AddModelError("Nombre", "Ya existe un método de pago con este nombre");
            }
            if (ModelState.IsValid)
            {

                _contenedorTrabajo.MetodosPago.Add(metodosPago);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(metodosPago);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            MetodosPago metodosPago = new MetodosPago();
            metodosPago = _contenedorTrabajo.MetodosPago.Get(id);
            if (metodosPago == null)
            {
                return NotFound();
            }
            return View(metodosPago);
        }

        [HttpPost]
        public IActionResult Edit(MetodosPago metodosPago)
        {
            var metodoPagoExistente = _contenedorTrabajo.MetodosPago.GetAll().FirstOrDefault(m =>
                m.Nombre.ToLower() == metodosPago.Nombre.ToLower() &&
                m.Id_metodoPago != metodosPago.Id_metodoPago);

            if (metodoPagoExistente != null)
            {
                ModelState.AddModelError("Nombre", "Ya existe otro método de pago con este nombre");
            }
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.MetodosPago.Update(metodosPago);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(metodosPago);
        }


        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()//GETALL
        {
            return Json(new { data = _contenedorTrabajo.MetodosPago.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.MetodosPago.Get(id);



            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Método de Pago" });
            }
            _contenedorTrabajo.MetodosPago.Remove(objFromDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "Método de Pago Borrado Exitosamente" });
        }
        #endregion
    }
}
