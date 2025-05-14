using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;

namespace SamsungV1.Areas.Almacen.Controllers
{
    [Area("Almacen")]
    [Authorize(Roles = "Admin,Almacen")]
    public class HistorialPreciosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public HistorialPreciosController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()//GETALL
        {

            //return Json(new { data = _contenedorTrabajo.HistorialPrecios.GetAll() });
            
            var data = _context.HistorialPrecios
                .Include(h => h.Producto)
                .Include(h => h.Usuario)
                .OrderByDescending(h => h.Fecha_Cambio)
                .ToList();

            return Json(new { data = data });
        }
        #endregion
    }
}
