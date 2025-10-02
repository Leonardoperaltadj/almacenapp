using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class EvidenciaAsignacion
    {
        [Key]
        public int IdEvidencia { get; set; }
        public int IdAsignacion { get; set; }
        [ForeignKey("IdAsignacion")]
        public Asignacion Asignacion { get; set; }
        public string Archivo { get; set; } = null!;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }

}
