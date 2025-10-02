using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<Personal> Personales { get; set; } = new List<Personal>();
    }
}
