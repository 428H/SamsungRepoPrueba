using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class InventarioActual
    {
        [Key]
        public int id_inventario { get; set; }

        [Required]
        public int id_producto { get; set; }

        [ForeignKey("id_producto")]
        public Producto Producto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cantidad Disponible")]
        public decimal cantidad_disponible { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Stock Mínimo")]
        public decimal stock_minimo { get; set; }


        [Display(Name = "Última Actualización")]
        public string ultima_actualizacion { get; set; }
    }
}
