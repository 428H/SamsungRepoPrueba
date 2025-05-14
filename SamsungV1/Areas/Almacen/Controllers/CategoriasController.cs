using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;

namespace SamsungV1.Areas.Almacen.Controllers
{
    [Area("Almacen")]
    [Authorize(Roles = "Admin,Almacen")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
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
        public IActionResult Create(Categoria categoria)
        {
            
            if (categoria.estado != "activo" && categoria.estado != "inactivo")
            {
                ModelState.AddModelError("estado", "El estado debe ser 'activo' o 'inactivo'");
            }

            var categoriaExistente = _contenedorTrabajo.Categoria.GetAll().FirstOrDefault(c =>
                c.nombre.ToLower() == categoria.nombre.ToLower());

            if (categoriaExistente != null)
            {
                ModelState.AddModelError("nombre", "Ya existe una categoría con este nombre");
            }

            if (ModelState.IsValid)
            {
                
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            var categoriaExistente = _contenedorTrabajo.Categoria.GetAll().FirstOrDefault(c =>
                c.nombre.ToLower() == categoria.nombre.ToLower() &&
                c.id_categoria != categoria.id_categoria);

            if (categoriaExistente != null)
            {
                ModelState.AddModelError("nombre", "Ya existe otra categoría con este nombre");
            }
            var categoriaActual = _contenedorTrabajo.Categoria.Get(categoria.id_categoria);
            if (categoriaActual.estado == "activo" && categoria.estado == "inactivo")
            {
                // Verificar si hay productos asociados a esta categoría
                var productosAsociados = _context.Producto.Any(p => p.id_categoria == categoria.id_categoria);

                if (productosAsociados)
                {
                    ModelState.AddModelError("estado", "No se puede desactivar esta categoría porque tiene productos asociados. Migre los productos a otra categoría primero.");
                }
            }

            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }




        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()//GETALL
        {
            return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Categoria.Get(id);



            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Categoria" });
            }
            _contenedorTrabajo.Categoria.Remove(objFromDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "Categoria Borrada Exitosamente" });
        }
        #endregion
    }
}
