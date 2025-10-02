using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class UsuarioRol
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public int IdRol { get; set; }
        [ForeignKey("IdUsuario")]
        [ValidateNever]
        public Usuario Usuario { get; set; }
        [ForeignKey("IdRol")]
        [ValidateNever]
        public Rol Rol { get; set; }
    }
}
