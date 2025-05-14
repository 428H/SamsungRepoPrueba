using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using Sistema.Models.ViewModels;

namespace SamsungV1.Areas.Almacen.Controllers
{
    [Area("Almacen")]
    [Authorize(Roles = "Admin,Almacen")]

    //[Authorize] 
    public class ProductosController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment,
        UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductoVM produVM = new ProductoVM()
            {
                Producto = new Sistema.Models.Producto(),
                InventarioActual = new Sistema.Models.InventarioActual(),
                MovimientoInventario = new Sistema.Models.MovimientoInventario(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias(),
                ListaTiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento()
            };
            return View(produVM);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoVM produVM)
        {
            try
            {
                var categoria = _contenedorTrabajo.Categoria.Get(produVM.Producto.id_categoria);

                // Verifica si la categoría está activa
                if (categoria == null || categoria.estado != "activo")
                {
                    
                    ModelState.AddModelError(string.Empty, "No se puede asociar un producto con una categoría inactiva");
                }

                produVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                produVM.ListaTiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento();

                // Obtener usuario
                var usuarioActual = await _userManager.GetUserAsync(User);
                if (usuarioActual == null)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo obtener el usuario actual");
                    return View(produVM);
                }

                //MovimientoInventario
                if (produVM.MovimientoInventario == null)
                    produVM.MovimientoInventario = new MovimientoInventario();

                produVM.MovimientoInventario.id_usuario = usuarioActual.Id;
                produVM.MovimientoInventario.fecha_movimiento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                produVM.MovimientoInventario.cantidad = produVM.InventarioActual?.cantidad_disponible ?? 0;

                ModelState.Remove("MovimientoInventario.id_usuario");
                ModelState.Remove("MovimientoInventario.fecha_movimiento");
                ModelState.Remove("MovimientoInventario.cantidad");

                
                if (!ModelState.IsValid)
                {
                    return View(produVM);
                }

                // imagen 
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Debe seleccionar una imagen para el producto");
                    return View(produVM);
                }

                // Procesar imagen
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                string nombreArchivo = Guid.NewGuid().ToString();// cadena de caracteres
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");

                if (!Directory.Exists(subidas)) Directory.CreateDirectory(subidas);

                var extension = Path.GetExtension(archivos[0].FileName);//accedemos a su nombre de archivo
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);//lo guardamos arriba
                }

                //producto
                produVM.Producto.ruta_imagen = @"imagenes\productos\" + nombreArchivo + extension;
                produVM.Producto.id_usuario = usuarioActual.Id;

                _contenedorTrabajo.Producto.Add(produVM.Producto);
                _contenedorTrabajo.save();

                //inventario 
                if (produVM.InventarioActual != null && produVM.InventarioActual.cantidad_disponible > 0)
                {
                   
                    produVM.InventarioActual.id_producto = produVM.Producto.id_producto;
                    produVM.InventarioActual.ultima_actualizacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _contenedorTrabajo.InventarioActual.Add(produVM.InventarioActual);
                    _contenedorTrabajo.save();
                    /// Registrar movimiento
                    if (produVM.MovimientoInventario.id_tipo_movimiento > 0)
                    {
                        var tipoMovimientoe = _contenedorTrabajo.TipoMovimientoI.Get(produVM.MovimientoInventario.id_tipo_movimiento);

                        // Verifica si la TipoMovimineto está activa
                        if (tipoMovimientoe == null || tipoMovimientoe.estado != "activo")
                        {
                            ModelState.AddModelError("MovimientoInventario.id_tipo_movimiento",
                                "No se puede utilizar un tipo de movimiento inactivo");
                            return View(produVM);
                        }

                        var selectedMovementValue = produVM.MovimientoInventario.id_tipo_movimiento.ToString();
                        var selectedMovement = produVM.ListaTiposMovimiento.FirstOrDefault(t => t.Value == selectedMovementValue);

                        // 
                        if (selectedMovement != null && selectedMovement.Text.ToLower().Contains("entrada"))//esto "entrada"le tenemos que cambiar
                                                                                                            //segun el nombre del moviemiento que queramos
                        {
                            var movimiento = new MovimientoInventario
                            {
                                id_producto = produVM.Producto.id_producto,
                                cantidad = produVM.InventarioActual.cantidad_disponible,
                                id_tipo_movimiento = produVM.MovimientoInventario.id_tipo_movimiento,
                                fecha_movimiento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                motivo = "Creación inicial de producto",
                                id_usuario = usuarioActual.Id
                            };

                            _contenedorTrabajo.MovimientoInventario.Add(movimiento);
                            _contenedorTrabajo.save();
                        }
                        else
                        {
                            
                            ModelState.AddModelError("MovimientoInventario.id_tipo_movimiento",
                                "Para la creación inicial del producto, debe seleccionar un tipo de movimiento de Entrada.");
                            return View(produVM);
                        }
                    }
                    else
                    {
                        
                        ModelState.AddModelError("MovimientoInventario.id_tipo_movimiento",
                            "Debe seleccionar un tipo de movimiento.");
                        return View(produVM);
                    }
                }

                TempData["Success"] = "Producto creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
               
                TempData["Error"] = $"Error al crear el producto: {ex.Message}";
                produVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                produVM.ListaTiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento();
                return View(produVM);
            }
        }

        //metodo para editar productos
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ProductoVM produVM = new ProductoVM()
            {
                Producto = new Sistema.Models.Producto(),
                InventarioActual = new Sistema.Models.InventarioActual(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                produVM.Producto = _contenedorTrabajo.Producto.Get(id.GetValueOrDefault());

                
                var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(
                    filter: i => i.id_producto == id.GetValueOrDefault()
                );

                if (inventario != null)
                {
                    produVM.InventarioActual = inventario;
                }
            }

            return View(produVM);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductoVM produVM)
        {
            var categoria = _contenedorTrabajo.Categoria.Get(produVM.Producto.id_categoria);

            //// Verificar si la categoría existe y está activa
            //if (categoria == null || categoria.estado != "activo")
            //{
            //    ModelState.AddModelError(string.Empty, "No se puede asociar un producto con una categoría inactiva");
            //} 
            if (ModelState.IsValid)
            {
                
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var productoDesdeBd = _contenedorTrabajo.Producto.Get(produVM.Producto.id_producto);

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, productoDesdeBd.ruta_imagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    produVM.Producto.ruta_imagen = @"imagenes\productos\" + nombreArchivo + extension;
                    produVM.Producto.fecha_creacion = DateTime.Now.ToString();
                }
                else
                {
                    produVM.Producto.ruta_imagen = productoDesdeBd.ruta_imagen;
                }

                // Actualizar el producto
                _contenedorTrabajo.Producto.Update(produVM.Producto);

                // Buscar inventario para producto
                var inventarioExistente = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(
                    filter: i => i.id_producto == produVM.Producto.id_producto
                );

                if (inventarioExistente != null)
                {
                    // Actualizar inventario existente
                    produVM.InventarioActual.id_inventario = inventarioExistente.id_inventario;
                    produVM.InventarioActual.id_producto = produVM.Producto.id_producto;
                    produVM.InventarioActual.ultima_actualizacion = DateTime.Now.ToString();

                    _contenedorTrabajo.InventarioActual.Update(produVM.InventarioActual);
                }
                else if (produVM.InventarioActual != null && produVM.InventarioActual.cantidad_disponible > 0)
                {
                    
                    produVM.InventarioActual.id_producto = produVM.Producto.id_producto;
                    produVM.InventarioActual.ultima_actualizacion = DateTime.Now.ToString();

                    _contenedorTrabajo.InventarioActual.Add(produVM.InventarioActual);
                }

                _contenedorTrabajo.save();

                return RedirectToAction(nameof(Index));
            }

            produVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(produVM);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            ProductoVM produVM = new ProductoVM()
            {
                Producto = new Sistema.Models.Producto(),
                InventarioActual = new Sistema.Models.InventarioActual(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                produVM.Producto = _context.Producto
                        .Include(p => p.Categoria)
                        .FirstOrDefault(p => p.id_producto == id.GetValueOrDefault());
                produVM.Producto = _contenedorTrabajo.Producto.Get(id.GetValueOrDefault());

                // Buscar inventario para el producto
                var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(
                    filter: i => i.id_producto == id.GetValueOrDefault()
                );

                if (inventario != null)
                {
                    produVM.InventarioActual = inventario;
                }
            }

            return View(produVM);
        }

        [HttpGet]
        public IActionResult AgregarProducto(int? id = null)
        {
            // producto y su inventario  

            var producto = _contenedorTrabajo.Producto.Get(id.GetValueOrDefault());
            if (producto == null)
            {
                return NotFound();
            }
            
            var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(i => i.id_producto == id);
            if (inventario == null)
            {
               
                TempData["Error"] = "No se encontró información de inventario para este producto";
                return RedirectToAction(nameof(Index));
            }

          
            var productoVM = new ProductoVM()
            {
                Producto = producto,
                InventarioActual = inventario,
                MovimientoInventario = new Sistema.Models.MovimientoInventario(),
                ListaTiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento()
            };

            return View(productoVM);
        }

        // Método POST para crear
        // cambios de stock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarProducto(int id, int cantidad, string tipoOperacion, int id_tipo_movimiento, string motivo)
        {
            try
            {
                
                if (cantidad <= 0)
                    return ManejarError("La cantidad debe ser mayor a cero", id);
                // Verificar que el tipo de movimiento esté activo
                var tipoMovimientoe = _contenedorTrabajo.TipoMovimientoI.Get(id_tipo_movimiento);
                if (tipoMovimientoe == null)
                    return ManejarError("Tipo de movimiento no encontrado", id);

                if (tipoMovimientoe.estado != "activo")
                    return ManejarError("No se puede utilizar un tipo de movimiento inactivo", id);

                var usuarioActual = await _userManager.GetUserAsync(User);
                var producto = _contenedorTrabajo.Producto.Get(id);
                var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(i => i.id_producto == id);
                var tipoMovimiento = _contenedorTrabajo.TipoMovimientoI.GetFirstOrderDefault(t => t.id_tipomovimiento == id_tipo_movimiento);

                // Verificar que todos los objetos existan
                if (usuarioActual == null)
                    return ManejarError("No se pudo obtener el usuario actual");

                if (producto == null)
                    return ManejarError("Producto no encontrado");

                if (inventario == null)
                    return ManejarError("No se encontró inventario para este producto");

                if (tipoMovimiento == null)
                    return ManejarError("El tipo de movimiento seleccionado no es válido", id);

                // Calcular nuevo stock
                decimal nuevoStock;
                if (tipoOperacion == "agregar")
                {
                    nuevoStock = inventario.cantidad_disponible + cantidad;
                }
                else // reducir
                {
                    if (inventario.cantidad_disponible < cantidad)
                        return ManejarError($"No hay suficiente stock para reducir. Stock actual: {inventario.cantidad_disponible}", id);

                    nuevoStock = inventario.cantidad_disponible - cantidad;
                }

                // guardR EL inventario
                inventario.cantidad_disponible = nuevoStock;
                inventario.ultima_actualizacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _contenedorTrabajo.InventarioActual.Update(inventario);

                // movimiento
                var movimiento = new Sistema.Models.MovimientoInventario
                {
                    id_producto = id,
                    cantidad = cantidad,
                    id_tipo_movimiento = id_tipo_movimiento,
                    fecha_movimiento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    motivo = motivo,
                    id_usuario = usuarioActual.Id
                };

                //agregar
                _contenedorTrabajo.MovimientoInventario.Add(movimiento);
                _contenedorTrabajo.save();

                
                TempData["Success"] = $"Stock {(tipoOperacion == "agregar" ? "aumentado" : "reducido")} correctamente. Nuevo stock: {nuevoStock}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var mensaje = "Error al actualizar el stock: " + ex.Message;
                if (ex.InnerException != null)
                    mensaje += " | " + ex.InnerException.Message;

                return ManejarError(mensaje, id);
            }
        }

        
        private IActionResult ManejarError(string mensaje, int? idProducto = null)
        {
            TempData["Error"] = mensaje;
            return idProducto.HasValue ?
                   RedirectToAction(nameof(AgregarProducto), new { id = idProducto }) :
                   RedirectToAction(nameof(Index));
        }

        #region llamadas API
        [HttpGet]
        public IActionResult GetAll()
        {
                var productos = _contenedorTrabajo.Producto.GetAll(includeProperties: "Categoria");
                var inventarios = _contenedorTrabajo.InventarioActual.GetAll()
                    .ToDictionary(i => i.id_producto);

               
                var resultados = productos.Select(p => {
                    
                    inventarios.TryGetValue(p.id_producto, out var inventario);

                    
                    return new
                    {
                        p.id_producto,
                        p.nombre,
                        p.modelo,
                        p.precio,
                        p.ruta_imagen,
                        p.especificaciones,
                        categoria = p.Categoria?.nombre,
                        tiene_stock = inventario != null,
                        stock_disponible = inventario?.cantidad_disponible ?? 0,
                        stock_minimo = inventario?.stock_minimo ?? 0
                    };
                });

                return Json(new { data = resultados });
           
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productoDesdeBd = _contenedorTrabajo.Producto.Get(id);
            if (productoDesdeBd == null)
            {
                return Json(new { success = false, message = "Error al Borrar: Producto no encontrado" });
            }

            if (User.IsInRole("Admin"))
            {
                try
                {
                    
                    var historialPrecios = _context.HistorialPrecios
                        .Where(h => h.Id_producto == id)
                        .ToList();

                    foreach (var historial in historialPrecios)
                    {
                        _context.HistorialPrecios.Remove(historial);
                    }

                    
                    _context.SaveChanges();

                    
                    var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(
                        filter: i => i.id_producto == id
                    );
                    if (inventario != null)
                    {
                        _contenedorTrabajo.InventarioActual.Remove(inventario);
                    }

                    
                    var movimientos = _contenedorTrabajo.MovimientoInventario.GetAll(
                        filter: m => m.id_producto == id
                    );
                    foreach (var movimiento in movimientos)
                    {
                        _contenedorTrabajo.MovimientoInventario.Remove(movimiento);
                    }

                    
                    _contenedorTrabajo.Producto.Remove(productoDesdeBd);
                    _contenedorTrabajo.save();

                    
                    string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
                    var rutaImagen = Path.Combine(rutaDirectorioPrincipal, productoDesdeBd.ruta_imagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    return Json(new { success = true, message = "Producto borrado correctamente" });
                }
                catch (Exception ex)
                {
                   
                    return Json(new
                    {
                        success = false,
                        message = $"Error al borrar el producto: {ex.Message}",
                        details = ex.InnerException?.Message 
                    });
                }
            }
            else
            {
                return Json(new { success = false, message = "No tiene permisos para borrar productos" });
            }
        }

        #endregion
    }
}
