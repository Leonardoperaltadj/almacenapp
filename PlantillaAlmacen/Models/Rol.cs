using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(255)]
        public string Descripcion { get; set; }

        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
