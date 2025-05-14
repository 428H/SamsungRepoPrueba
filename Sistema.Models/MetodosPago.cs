using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class MetodosPago
    {
        [Key]
        public int Id_metodoPago { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del Método")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Ingresar Breve descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}

