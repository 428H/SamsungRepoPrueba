using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using System.Security.Claims;
namespace SamsungV1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {//opcion 1
            return View(_contenedorTrabajo.Usuario.GetAllUsers());

            ////opcion 2
            //var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            //var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //return View(_contenedorTrabajo.Usuario.GetAllUsers(u => u.Id != usuarioActual.Value));
        }



        [HttpGet]
        public IActionResult Details(string id)
        {
            var usuario = _contenedorTrabajo.Usuario.GetUser(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Bloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Usuario.GetAllUsers() });
        }
        #endregion
    }
}
