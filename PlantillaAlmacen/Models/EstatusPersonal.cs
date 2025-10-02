using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class EstatusPersonal
    {
        [Key]
        public int IdEstatusPersonal { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<Personal> Personales { get; set; } = new List<Personal>();
    }
}
