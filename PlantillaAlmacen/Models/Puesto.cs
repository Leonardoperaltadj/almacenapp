using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Puesto
    {
        [Key]
        public int IdPuesto { get; set; }

        [Required, StringLength(80)]
        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;
        public ICollection<Personal> Personales { get; set; }
    }
}
