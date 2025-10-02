using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class UnidadMedida
    {
        [Key] public int IdUnidadMedida { get; set; }
        [Required, StringLength(20)] public string Clave { get; set; }
        [Required, StringLength(50)] public string Nombre { get; set; }
        public bool Estatus { get; set; } = true;
    }
}
