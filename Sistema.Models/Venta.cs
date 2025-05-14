using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class Venta
    {
        [Key]
        [Display(Name = "ID Venta")]
        public int Id_venta { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        [Display(Name = "Cliente")]
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "El subtotal es requerido")]
        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El método de pago es requerido")]
        [Display(Name = "Método de Pago")]
        public int Id_metodo_pago { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Required(ErrorMessage = "El usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Id_usuario { get; set; }

        [Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres")]
        public string Notas { get; set; }

        // Propiedades de navegación
        [ForeignKey("Id_cliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("Id_metodo_pago")]
        public MetodosPago MetodoPago { get; set; }

        [ForeignKey("Id_usuario")]
        public ApplicationUser Usuario { get; set; }
    }
}
