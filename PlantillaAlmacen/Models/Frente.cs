using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Frente
    {
        [Key]
        public int IdFrente { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(200)]
        public bool Activo { get; set; } = true;
        public ICollection<Personal> Personales { get; set; }
    }
}
