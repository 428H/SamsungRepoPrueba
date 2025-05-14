using Microsoft.AspNetCore.Identity;
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
    public class UsuarioRepository : Repository<ApplicationUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void BloquearUsuario(string id)
        {
            var usuarioDesdeBd = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            usuarioDesdeBd.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void DesbloquearUsuario(string id)
        {
            var usuarioDesdeBd = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            usuarioDesdeBd.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
