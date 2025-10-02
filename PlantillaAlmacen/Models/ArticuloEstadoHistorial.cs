using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class ArticuloEstadoHistorial
    {
        [Key] public int IdHistorial { get; set; }
        public int IdArticulo { get; set; }
        public int IdEstatusArticuloAnterior { get; set; }
        public int IdEstatusArticuloNuevo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int? IdUsuario { get; set; }
        [StringLength(255)] public string? Motivo { get; set; }
        [ForeignKey(nameof(IdArticulo))] public Articulo Articulo { get; set; }
        [ForeignKey(nameof(IdUsuario))] public Usuario? Usuario { get; set; }
    }
}
