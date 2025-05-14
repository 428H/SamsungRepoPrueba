using Microsoft.AspNetCore.Mvc.Rendering;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaCategorias()
        {
            return _db.Categoria.Where(i => i.estado == "activo")
                .Select(i => new SelectListItem()
                {
                    Text = i.nombre,
                    Value = i.id_categoria.ToString()
                });
        }
        /*public IEnumerable<SelectListItem> GetListaCategorias()
{
   return _db.Categoria.Select(i => new SelectListItem()
   {
       Text = i.Nombre,
       Value = i.Id.ToString()
   });
}*/
        public void Update(Categoria categoria)
        {

            var objDesdeDb = _db.Categoria.FirstOrDefault(s => s.id_categoria == categoria.id_categoria);
            if (objDesdeDb != null)
            {
                objDesdeDb.nombre = categoria.nombre;
                objDesdeDb.descripcion = categoria.descripcion;
                objDesdeDb.estado = categoria.estado;
                
            }
            else
            {
                
                throw new Exception($"No se encontró la categoría con ID {categoria.id_categoria}");
            }

            _db.SaveChanges();
        }
    }
}
