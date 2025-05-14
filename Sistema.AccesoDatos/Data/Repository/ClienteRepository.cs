using Microsoft.AspNetCore.Mvc.Rendering;
using SamsungV1.Data;
using Sistema.AccesoDatos.Data.Repository.IRepository;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository
{
    public class ClienteRepository: Repository<Cliente>, IClienteRepository
    {
        private readonly ApplicationDbContext _db;
        public ClienteRepository( ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Cliente cliente)
        {

            var objDesdeDb = _db.Cliente.FirstOrDefault(s => s.Id_cliente == cliente.Id_cliente);
            objDesdeDb.nombre = cliente.nombre;
            objDesdeDb.apellido = cliente.apellido;
            objDesdeDb.dui = cliente.dui;
            objDesdeDb.telefono = cliente.telefono;
            objDesdeDb.email = cliente.email;
            objDesdeDb.estado = cliente.estado;
            //objDesdeDb.fecha_registro = cliente.fecha_registro;
        }

        public IEnumerable<SelectListItem> GetAllClientes()
        {
            return _db.Cliente
                .Where(c => c.estado == "activo")
                .Select(i => new SelectListItem()
                {
                    Text = i.nombre + " " + i.apellido + " (" + i.dui + ")",
                    Value = i.Id_cliente.ToString()
                });
        }

        
    }
    
}
