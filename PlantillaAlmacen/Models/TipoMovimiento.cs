using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class TipoMovimiento
    {
        [Key]
        public int IdTipoMovimiento { get; set; }
        [StringLength(50)]
        [Required]
        public string Nombre { get; set; }
        public bool Estatus { get; set; } = true;

    }
}
