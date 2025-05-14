using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class MovimientoInventarioRepository : Repository<MovimientoInventario>, IMovimientoInventarioRepository
    {
        private readonly ApplicationDbContext _db;

        public MovimientoInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

     

        public void Update(MovimientoInventario movimientoInventario)
        {
            throw new NotImplementedException();
        }

    }
}
