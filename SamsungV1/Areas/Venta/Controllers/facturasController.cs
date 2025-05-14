using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Resend;
using Rotativa.AspNetCore;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;

namespace SamsungV1.Areas.Venta.Controllers
{
    [Area("Venta")]
    [Authorize(Roles = "Admin, Venta")]
    public class facturasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;
        private readonly IResend _resend;
       

        public facturasController(IContenedorTrabajo contenedorTrabajo, ApplicationDbContext context, IResend resend )
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
            _resend = resend;
            
        }

        public IActionResult Index()
        {
            //return new ViewAsPdf();
            return View();
        }
       

    [HttpGet]
    public async Task<IActionResult> GenerarPDF(int id)
    {
        try
        {
            // Obtener factura
            var factura = _context.Facturas.Find(id);
            if (factura == null)
                return NotFound();

            var venta = _context.Ventas.FirstOrDefault(v => v.Id_venta == factura.Id_venta);
            var cliente = _context.Cliente.FirstOrDefault(c => c.Id_cliente == factura.Id_cliente);
            var usuario = _context.ApplicationUsers.FirstOrDefault(u => u.Id == factura.Id_usuario);

            //detalles de productos
            var detalles = (from detalle in _context.DetallesVenta
                            join producto in _context.Producto on detalle.Id_producto equals producto.id_producto
                            where detalle.Id_venta == factura.Id_venta
                            select new
                            {
                                Producto = producto.nombre,
                                Cantidad = detalle.Cantidad,
                                PrecioUnitario = detalle.Precio_unitario,
                                Subtotal = detalle.Cantidad * detalle.Precio_unitario
                            }).ToList();

            // Crear modelo vista
            var model = new
            {
                Numero = factura.Numero,
                FechaEmision = factura.Fecha_emision.ToString(),
                Cliente = cliente?.nombre ?? "Sin cliente",
                Usuario = usuario?.UserName ?? "Sin usuario",
                Total = venta?.Total ?? 0,
                Detalles = detalles
            };

               
                var htmlCorreo = $@"
        <html>
            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;'>
                <div style='text-align: center; margin-bottom: 20px;'>
                    <h1 style='color: #1a73e8;'>Samsung Store</h1>
                    <div style='border-top: 1px solid #eee; border-bottom: 1px solid #eee; padding: 10px;'>
                        <h2>Factura #{model.Numero}</h2>
                    </div>
                </div>
        
                <div style='margin-bottom: 20px;'>
                    <table style='width: 100%; border-spacing: 0; font-size: 14px;'>
                        <tr>
                            <td><strong>Fecha:</strong></td>
                            <td>{model.FechaEmision}</td>
                        </tr>
                        <tr>
                            <td><strong>Cliente:</strong></td>
                            <td>{model.Cliente}</td>
                        </tr>
                        <tr>
                            <td><strong>Vendedor:</strong></td>
                            <td>{model.Usuario}</td>
                        </tr>
                    </table>
                </div>
        
                <table style='width: 100%; border-collapse: collapse; font-size: 14px;'>
                    <tr style='background-color: #f5f5f5;'>
                        <th style='padding: 8px; text-align: left;'>Producto</th>
                        <th style='padding: 8px; text-align: center;'>Cant.</th>
                        <th style='padding: 8px; text-align: right;'>Precio</th>
                        <th style='padding: 8px; text-align: right;'>Subtotal</th>
                    </tr>";

                        foreach (var item in model.Detalles)
                        {
                            htmlCorreo += $@"
                    <tr style='border-bottom: 1px solid #eee;'>
                        <td style='padding: 8px;'>{item.Producto}</td>
                        <td style='padding: 8px; text-align: center;'>{item.Cantidad}</td>
                        <td style='padding: 8px; text-align: right;'>${string.Format("{0:0.00}", item.PrecioUnitario)}</td>
                        <td style='padding: 8px; text-align: right;'>${string.Format("{0:0.00}", item.Subtotal)}</td>
                    </tr>";
                        }

                        htmlCorreo += $@"
                </table>
        
                <div style='text-align: right; margin-top: 20px;'>
                    <strong style='font-size: 16px;'>Total: ${string.Format("{0:0.00}", model.Total)}</strong>
                </div>
        
                <div style='margin-top: 30px; color: #666; font-size: 13px;'>
                    <p>Gracias por su compra en Samsung Store.</p>
                    <p>Para cualquier consulta, contáctenos al (503) 2222-3333.</p>
                </div>
        
                <div style='text-align: center; margin-top: 30px; font-size: 12px; color: #999;'>
                    <p>Samsung Store &copy; 2025</p>
                </div>
            </body>
        </html>";


                
                var message = new EmailMessage();
                message.From = "Samsung Store <onboarding@resend.dev>";
                message.To.Add("xiomahrc@gmail.com");
                message.Subject = "Factura electrónica - Samsung Store";
                message.HtmlBody = htmlCorreo;
                await _resend.EmailSendAsync(message);


                
                return new ViewAsPdf("~/Views/Shared/_FacturaPDF.cshtml", model)
                {
                    FileName = $"Factura_{factura.Numero}.pdf",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar PDF: {ex.Message}");
        }
    }
        

        #region llamadas API
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = new List<object>();

               
                var facturas = _context.Facturas.ToList();

                foreach (var factura in facturas)
                {
                    
                    var venta = _context.Ventas.FirstOrDefault(v => v.Id_venta == factura.Id_venta);
                    var cliente = _context.Cliente.FirstOrDefault(c => c.Id_cliente == factura.Id_cliente);
                    var usuario = _context.ApplicationUsers.FirstOrDefault(u => u.Id == factura.Id_usuario);

                   
                    var detalles = (from detalle in _context.DetallesVenta
                                    join producto in _context.Producto on detalle.Id_producto equals producto.id_producto
                                    where detalle.Id_venta == factura.Id_venta
                                    select new
                                    {
                                        nombre_producto = producto.nombre,
                                        cantidad = detalle.Cantidad,
                                        precio = detalle.Precio_unitario
                                    }).ToList();

                   
                    string resumenProductos = string.Join(", ", detalles.Select(d => d.nombre_producto));
                    string resumenCantidades = string.Join(", ", detalles.Select(d => d.cantidad.ToString()));

                    
                    result.Add(new
                    {
                        id_factura = factura.Id_factura,
                        numero = factura.Numero,
                        id_venta = factura.Id_venta,
                        id_cliente = factura.Id_cliente,
                        cliente = cliente?.nombre ?? "Sin cliente",
                        fecha_emision = factura.Fecha_emision,
                        id_usuario = factura.Id_usuario,
                        usuario = usuario?.UserName ?? "Sin usuario",
                        subtotal = venta?.Subtotal ?? 0,
                        total = venta?.Total ?? 0,
                        productos = resumenProductos,
                        cantidades = resumenCantidades
                    });
                }

                return Json(new { data = result });
            }
            catch (System.Exception ex)
            {
                return Json(new { error = ex.Message, data = new object[] { } });
            }
        }


        #endregion
    }
}
