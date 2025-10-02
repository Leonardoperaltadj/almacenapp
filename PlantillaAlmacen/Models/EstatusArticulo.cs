using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class EstatusArticulo
    {
        [Key]
        public int IdEstatusArticulo { get; set; }
        [StringLength(50)]
        [Required]
        public string Nombre { get; set; }
        public bool Estatus { get; set; } = true;
    }
}
