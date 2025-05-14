using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Sistema.Models
{
    
    public class Categoria
    {
        [Key]
        public int id_categoria { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [Display(Name = "Nombre de Categoria")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        [Display(Name = "Descripción")]
        [StringLength(150, ErrorMessage = "La descripción no puede exceder los 150 caracteres")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        [RegularExpression("^(activo|inactivo)$", ErrorMessage = "El estado debe ser 'activo' o 'inactivo'")]
        public string estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string fecha_creacion { get; set; }
    }
}
