using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class Cliente
    {
        [Key]
        [Display(Name = "ID Cliente")]
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "Ingrese los nombres del cliente")]
        [Display(Name = "Nombres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜ\s]+$", ErrorMessage = "El campo {0} solo puede contener letras.")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Ingrese los apellidos del cliente")]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜ\s]+$", ErrorMessage = "El campo {0} solo puede contener letras.")]
        public string apellido { get; set; }

        [Required(ErrorMessage = "Ingrese el número de DUI")]
        [Display(Name = "DUI")]
        [RegularExpression(@"^\d{8}-\d{1}$", ErrorMessage = "El DUI debe tener el formato 00000000-0")]
        public string dui { get; set; }

        [Required(ErrorMessage = "Ingrese el número de teléfono")]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Ingrese un correo electrónico")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string email { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Display(Name = "Fecha de registro")]
        public string fecha_registro { get; set; }
    }
}
