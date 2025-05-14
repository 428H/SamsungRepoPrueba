using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class Factura
    {
        [Key]
        [Display(Name = "ID Factura")]
        public int Id_factura { get; set; }

        [Required(ErrorMessage = "El número de factura es requerido")]
        [Display(Name = "Número de Factura")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "La venta es requerida")]
        [Display(Name = "Venta")]
        public int Id_venta { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        [Display(Name = "Cliente")]
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es requerida")]
        [Display(Name = "Fecha de Emisión")]
        public string Fecha_emision { get; set; }

        [Required(ErrorMessage = "El usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Id_usuario { get; set; }

        // Propiedades de navegación
        [ForeignKey("Id_venta")]
        public Venta Venta { get; set; }

        [ForeignKey("Id_cliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("Id_usuario")]
        public ApplicationUser Usuario { get; set; }
    }
}
