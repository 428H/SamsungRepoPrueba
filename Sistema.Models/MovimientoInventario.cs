using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class MovimientoInventario
    {
        [Key]
        public int id_movimiento { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Column("id_producto")]  
        public int id_producto { get; set; }

        [ForeignKey("id_producto")]  
        public virtual Producto Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public decimal cantidad { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [Display(Name = "Tipo de Movimiento")]
        [Column("id_tipo_movimiento")]  
        public int id_tipo_movimiento { get; set; }

        [ForeignKey("id_tipo_movimiento")] 
        public virtual TipoMovimientoI TipoMovimiento { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha de Movimiento")]
        public string fecha_movimiento { get; set; }  

        [Display(Name = "Motivo")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres")]
        public string motivo { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Registrado por")]
        [Column("id_usuario")] 
        public string id_usuario { get; set; }

        [ForeignKey("id_usuario")]  
        public virtual ApplicationUser Usuario { get; set; }
    }

}
