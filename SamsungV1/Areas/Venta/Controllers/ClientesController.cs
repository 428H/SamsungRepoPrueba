using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;


namespace SamsungV1.Areas.Venta.Controllers
{
    [Area("Venta")]
    [Authorize(Roles = "Admin, Venta")]
   
    public class ClientesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;
        public ClientesController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;

        }
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
        public IActionResult Create(Cliente cliente)
        {

            if (cliente.estado != "activo" && cliente.estado != "inactivo")
            {
                ModelState.AddModelError("estado", "El estado debe ser 'activo' o 'inactivo'");
            }
            var clienteExistente = _contenedorTrabajo.Cliente.GetAll().FirstOrDefault(c =>
                c.dui == cliente.dui);

            if (clienteExistente != null)
            {
                ModelState.AddModelError("dui", "Ya existe un cliente registrado con este DUI");
            }
            if (ModelState.IsValid)
            {

                _contenedorTrabajo.Cliente.Add(cliente);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        //edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Cliente cliente = new Cliente();
            cliente = _contenedorTrabajo.Cliente.Get(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            var clienteExistente = _contenedorTrabajo.Cliente.GetAll().FirstOrDefault(c =>
                c.dui == cliente.dui &&
                c.Id_cliente != cliente.Id_cliente);

            if (clienteExistente != null)
            {
                ModelState.AddModelError("dui", "Ya existe otro cliente registrado con este DUI");
            }
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Cliente.Update(cliente);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        #region llamadas API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Cliente.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Cliente.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando cliente" });
            }

            _contenedorTrabajo.Cliente.Remove(objFromDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "Cliente borrado exitosamente" });
        }

        #endregion
    }
}
