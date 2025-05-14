using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
   public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Campo Requerido")]
        public string Nombre { get; set; }
    }
}
