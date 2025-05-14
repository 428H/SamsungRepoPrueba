using Microsoft.AspNetCore.Identity;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        ApplicationUser GetUser(string id);

        void BloquearUsuario(string id);
        void DesbloquearUsuario(string id);
    }
}
