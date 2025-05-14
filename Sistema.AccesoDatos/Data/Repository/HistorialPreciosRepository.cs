using Microsoft.EntityFrameworkCore;
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
    public class HistorialPreciosRepository : Repository<HistorialPrecios>, IHistorialPreciosRepository
    {
        private readonly ApplicationDbContext _db;

        public HistorialPreciosRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
