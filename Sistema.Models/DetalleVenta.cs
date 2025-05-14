using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class DetalleVenta
    {
        [Key]
        [Display(Name = "ID Detalle")]
        public int Id_detalle { get; set; }

        [Required(ErrorMessage = "La venta es requerida")]
        [Display(Name = "Venta")]
        public int Id_venta { get; set; }

        [Required(ErrorMessage = "El producto es requerido")]
        [Display(Name = "Producto")]
        public int Id_producto { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Display(Name = "Cantidad")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Display(Name = "Precio Unitario")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio_unitario { get; set; }

      
        [ForeignKey("Id_venta")]
        public Venta Venta { get; set; }

        [ForeignKey("Id_producto")]
        public Producto Producto { get; set; }
    }
}
