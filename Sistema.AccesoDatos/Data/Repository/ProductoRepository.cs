using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Producto producto)
        {
            var objDesdeDb = _db.Producto.FirstOrDefault(s => s.id_producto == producto.id_producto);

            if (objDesdeDb != null)
            {
                if (objDesdeDb.precio != producto.precio)
                {
                    // Crear registro en historial de precios
                    var historialPrecio = new HistorialPrecios
                    {
                        Id_producto = producto.id_producto,
                        Precio_Anterior = objDesdeDb.precio,
                        Precio_Nuevo = producto.precio,
                        Fecha_Cambio = DateTime.Now.ToString(),
                        Motivo = "Actualización de precio",
                        Id_Usuario = producto.id_usuario
                    };

                    _db.HistorialPrecios.Add(historialPrecio);
                }
                // Actualizar todas las propiedades
                objDesdeDb.modelo = producto.modelo;
                objDesdeDb.nombre = producto.nombre;
                objDesdeDb.descripcion = producto.descripcion;
                objDesdeDb.especificaciones = producto.especificaciones;
                objDesdeDb.precio = producto.precio;
                objDesdeDb.costo = producto.costo;
                objDesdeDb.id_categoria = producto.id_categoria;
                objDesdeDb.ruta_imagen = producto.ruta_imagen;
                objDesdeDb.estado = producto.estado;

                
                // objDesdeDb.fecha_creacion = producto.fecha_creacion;
                // objDesdeDb.id_usuario = producto.id_usuario;

               
                // _db.SaveChanges() 
            }
        }

    }
}
