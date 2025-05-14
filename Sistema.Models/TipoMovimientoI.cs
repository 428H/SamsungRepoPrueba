using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class TipoMovimientoI
    {
        [Key]
        public int id_tipomovimiento { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre del tipo de movimiento es obligatorio")]
        [StringLength(100)]
        public string nombre { get; set; }

        [Display(Name = "Afecta Stock")]
        [Required]
        [StringLength(1)]
        [RegularExpression("^[+-]$", ErrorMessage = "El valor debe ser '+' o '-'")]
        public string afecta_stock { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        [Display(Name = "Descripción")]
        [StringLength(255)]
        public string descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required]
        public string estado { get; set; }
    }
}
