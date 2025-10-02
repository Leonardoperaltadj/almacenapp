using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Permiso
    {
        [Key]
        public int IdPermiso { get; set; }
        [StringLength(100)]
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
