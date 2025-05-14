using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;

namespace SamsungV1.Areas.Almacen.Controllers
{
    [Area("Almacen")]
    [Authorize(Roles = "Admin,Almacen")]
    public class MovimientosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public MovimientosController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
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
        public IActionResult GetAll()
        {
            try
            {
                
                var movimientos = _context.MovimientoInventario
                    .Include(m => m.Producto)               
                    .Include(m => m.TipoMovimiento)        
                    .ToList();

                return Json(new { data = movimientos });
            }
            catch (System.Exception ex)
            {
                return Json(new { error = ex.Message, data = new object[] { } });
            }
        }
        #endregion
    }
}
