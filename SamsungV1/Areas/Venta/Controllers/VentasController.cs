using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using Sistema.Models.ViewModels;
using System.Text.Json;
using VentaModel = Sistema.Models.Venta;

namespace SamsungV1.Areas.Venta.Controllers
{
    [Area("Venta")]
    public class VentasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public VentasController(
            IContenedorTrabajo contenedorTrabajo,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var usuario = await _userManager.GetUserAsync(User);

            //  tipos de movimiento
            var tiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento();

           
            var tiposSalida = tiposMovimiento.Where(t => t.Text.ToLower().Contains("salida") || t.Text.ToLower().Contains("venta")).ToList();

            VentaVM ventaVM = new VentaVM()
            {
                Venta = new VentaModel(),
                ClientesLista = _contenedorTrabajo.Cliente.GetAllClientes(),
                MetodosPagoLista = _contenedorTrabajo.MetodosPago.GetAllMetodosPago(),
                ListaTiposMovimiento = tiposSalida, 
                Clientes = _contenedorTrabajo.Cliente.GetAll(),
                Productos = _contenedorTrabajo.Producto.GetAll(
                            //filter: p => p.estado == "activo",
                        includeProperties: "Categoria")
            };

            ventaVM.Venta.Fecha = DateTime.Now.ToString();

            if (usuario != null)
            {
                ventaVM.Venta.Id_usuario = usuario.Id;
            }

            return View(ventaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaVM ventaVM)
        {
            if (!ModelState.IsValid)
            {
                RecargarListas(ventaVM);
                return View(ventaVM);
            }

            // Validar tipo de movimiento
            if (ventaVM.IdTipoMovimientoSeleccionado <= 0)
            {
                ModelState.AddModelError("IdTipoMovimientoSeleccionado", "Debe seleccionar un tipo de movimiento para la salida de inventario");
                RecargarListas(ventaVM);
                return View(ventaVM);
            }

            // estado
            var tipoMovimiento = _contenedorTrabajo.TipoMovimientoI.Get(ventaVM.IdTipoMovimientoSeleccionado);
            if (tipoMovimiento == null || tipoMovimiento.estado != "activo")
            {
                ModelState.AddModelError("IdTipoMovimientoSeleccionado", "El tipo de movimiento seleccionado no es válido o está inactivo");
                RecargarListas(ventaVM);
                return View(ventaVM);
            }
            // productos seleccionados
            if (ventaVM.ProductosSeleccionados == null || ventaVM.ProductosSeleccionados.Count == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto para crear la venta");
                RecargarListas(ventaVM);
                return View(ventaVM);
            }
            //  cliente
            if (ventaVM.Venta.Id_cliente <= 0)
            {
                ModelState.AddModelError("Venta.Id_cliente", "Debe seleccionar un cliente");
                RecargarListas(ventaVM);
                return View(ventaVM);
            }

            try
            {
                
                var usuarioActual = await _userManager.GetUserAsync(User);
                if (usuarioActual == null)
                {
                    ModelState.AddModelError("", "No se pudo obtener el usuario actual");
                    RecargarListas(ventaVM);
                    return View(ventaVM);
                }

               
                ventaVM.Venta.Id_usuario = usuarioActual.Id;
                ventaVM.Venta.Fecha = DateTime.Now.ToString();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //  Guardar la venta
                        _contenedorTrabajo.Venta.Add(ventaVM.Venta);
                        _contenedorTrabajo.save();
                        int idVenta = ventaVM.Venta.Id_venta;

                        // 
                        foreach (var detalle in ventaVM.ProductosSeleccionados)
                        {
                            var producto = _contenedorTrabajo.Producto.Get(detalle.Id);

                            if (producto == null)
                            {
                                ModelState.AddModelError("", $"El producto con ID {detalle.Id} no existe.");
                                RecargarListas(ventaVM);
                                return View(ventaVM);
                            }

                           
                            if (producto.estado != "activo")
                            {
                                ModelState.AddModelError("", $"El producto '{producto.nombre}' no está activo y no puede ser vendido.");
                                RecargarListas(ventaVM);
                                return View(ventaVM);
                            }
                            var detalleVenta = new DetalleVenta
                            {
                                Id_venta = idVenta,
                                Id_producto = detalle.Id,
                                Cantidad = detalle.Cantidad,
                                Precio_unitario = detalle.Precio
                            };

                            _contenedorTrabajo.DetalleVenta.Add(detalleVenta);

                            // 
                            var inventario = _contenedorTrabajo.InventarioActual.GetFirstOrderDefault(
                                filter: i => i.id_producto == detalle.Id
                            );

                            if (inventario == null)
                            {
                                throw new Exception($"No se encontró inventario para el producto: {detalle.Nombre}");
                            }

                            if (inventario.cantidad_disponible < detalle.Cantidad)
                            {
                                throw new Exception($"Stock insuficiente para el producto: {detalle.Nombre}. Stock actual: {inventario.cantidad_disponible}");
                            }

                            // Reducir inventario
                            inventario.cantidad_disponible -= detalle.Cantidad;
                            inventario.ultima_actualizacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            _contenedorTrabajo.InventarioActual.Update(inventario);

                            var movimiento = new MovimientoInventario
                            {
                                id_producto = detalle.Id,
                                cantidad = detalle.Cantidad,
                                id_tipo_movimiento = ventaVM.IdTipoMovimientoSeleccionado,
                                fecha_movimiento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                motivo = $"Venta #{idVenta}",
                                id_usuario = usuarioActual.Id
                            };

                            _contenedorTrabajo.MovimientoInventario.Add(movimiento);
                        }

                        var numeroFactura = GenerarNumeroFacturaAleatorio();
                        var factura = new Factura
                        {
                            Numero = numeroFactura,
                            Id_venta = idVenta,
                            Id_cliente = ventaVM.Venta.Id_cliente,
                            Fecha_emision = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Id_usuario = usuarioActual.Id
                        };

                        _context.Facturas.Add(factura);

                        _contenedorTrabajo.save();
                        transaction.Commit();

                        TempData["Success"] = "Venta registrada correctamente";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                        RecargarListas(ventaVM);
                        return View(ventaVM);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                RecargarListas(ventaVM);
                return View(ventaVM);
            }
        }
        private string GenerarNumeroFacturaAleatorio()
        {
            
            Random random = new Random();
            int numeroAleatorio = random.Next(10000000, 99999999);

            
            while (_context.Facturas.Any(f => f.Numero == numeroAleatorio.ToString()))
            {
                numeroAleatorio = random.Next(10000000, 99999999);
            }

            return numeroAleatorio.ToString();
        }

        private void RecargarListas(VentaVM ventaVM)
        {
            ventaVM.ClientesLista = _contenedorTrabajo.Cliente.GetAllClientes();
            ventaVM.MetodosPagoLista = _contenedorTrabajo.MetodosPago.GetAllMetodosPago();

            // 
            var tiposMovimiento = _contenedorTrabajo.TipoMovimientoI.GetListaTiposMovimiento();
            var tiposSalida = tiposMovimiento.Where(t => t.Text.ToLower().Contains("salida") ||
                                                       t.Text.ToLower().Contains("venta")).ToList();
            ventaVM.ListaTiposMovimiento = tiposSalida;

            // 
            ventaVM.Clientes = _contenedorTrabajo.Cliente.GetAll();
            ventaVM.Productos = _contenedorTrabajo.Producto.GetAll(includeProperties: "Categoria");
        }

        #region llamadas a la api
        [HttpGet]
        [HttpGet]
        public IActionResult GetAll()
        {
            var productos = _contenedorTrabajo.Producto.GetAll(
                filter: p => p.estado == "activo",
                includeProperties: "Categoria");

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
                    p.estado,
                    categoria = p.Categoria?.nombre,
                    tiene_stock = inventario != null,
                    stock_disponible = inventario?.cantidad_disponible ?? 0,
                    stock_minimo = inventario?.stock_minimo ?? 0 
                };
            });

            return Json(new { data = resultados });
        }


        #endregion
    }
}