using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [StringLength(255)]
        [Required]
        public string Contrasena { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public int IdPersonal { get; set; }
        public bool Estatus { get; set; } = true;
        public DateTime FechaAlta { get; set; }
        [ForeignKey("IdPersonal")]
        [ValidateNever]
        public Personal Personal { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
