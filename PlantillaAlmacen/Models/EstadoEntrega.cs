using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class EstadoEntrega
    {
        [Key]
        public int IdEstadoEntrega { get; set; }
        [StringLength(50)]
        [Required]
        public string Nombre { get; set; }
        public bool Estatus { get; set; }
    }
}
