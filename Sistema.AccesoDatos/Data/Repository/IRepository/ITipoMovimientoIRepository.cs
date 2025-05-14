using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.AccesoDatos.Data.Repository.IRepository
{
    public interface ITipoMovimientoIRepository: IRepository<TipoMovimientoI>
    {

        void Update(TipoMovimientoI tipoMovimientoI);

        IEnumerable<SelectListItem> GetListaTiposMovimiento();
    }
}
