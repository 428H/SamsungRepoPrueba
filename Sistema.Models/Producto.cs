using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;



namespace Sistema.Models
{
    public class Producto
    {
        [Key]
        public int id_producto { get; set; }

        [Required(ErrorMessage = "El modelo del producto es obligatorio")]
        [Display(Name = "Modelo")]
        [StringLength(100, ErrorMessage = "El modelo no puede exceder los 100 caracteres")]
        public string modelo { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        [Display(Name = "Descripción")]
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder los 2000 caracteres")]
        public string descripcion { get; set; }

        [Display(Name = "Especificaciones")]
        public string especificaciones { get; set; } 

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal precio { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Display(Name = "Costo")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a cero")]
        public decimal costo { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int id_categoria { get; set; }

        [ForeignKey("id_categoria")]
        public virtual Categoria Categoria { get; set; }

        [Display(Name = "Imagen")]
        public string ruta_imagen { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        [RegularExpression("^(activo|inactivo)$", ErrorMessage = "El estado debe ser 'activo' o 'inactivo'")]
        public string estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string fecha_creacion { get; set; }

        [Required]
        [Display(Name = "Creado por")]
        public string id_usuario { get; set; }

        
        [ForeignKey("id_usuario")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}
