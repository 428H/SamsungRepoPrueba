using Sistema.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class HistorialPrecios
    {
        [Key]
        public int Id_historial { get; set; }

        [Required]
        public int Id_producto { get; set; }

        [ForeignKey("Id_producto")]
        public virtual Producto Producto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio_Anterior { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio_Nuevo { get; set; }

        [Required]
        public string Fecha_Cambio { get; set; }

        [StringLength(500)]
        public string Motivo { get; set; }

        [Required]
        public string Id_Usuario { get; set; }

        [ForeignKey("Id_Usuario")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}
