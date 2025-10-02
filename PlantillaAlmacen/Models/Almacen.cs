using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Almacen
    {
        [Key]
        public int IdAlmacen { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string? Ubicacion { get; set; }

        public bool Estatus { get; set; } = true;
        public ICollection<Articulo> Articulos { get; set; }
    }

}
