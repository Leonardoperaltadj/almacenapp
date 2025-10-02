using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class RolPermiso
    {
        [Required]
        public int IdRol { get; set; }
        [Required]
        public int IdPermiso { get; set; }
        [ForeignKey("IdRol")]
        [ValidateNever]
        public Rol Rol { get; set; }
        [ForeignKey("IdPermiso")]
        [ValidateNever]
        public Permiso Permiso { get; set; }
    }
}
