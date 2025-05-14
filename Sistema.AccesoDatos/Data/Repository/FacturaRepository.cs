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
    public class FacturaRepository : Repository<Factura>, IFacturaRepository
    {

        private readonly ApplicationDbContext _db;

        public FacturaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Factura factura)
        {
            
        }
    }
}
