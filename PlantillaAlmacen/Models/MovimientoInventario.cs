using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class MovimientoInventario
    {
        [Key]
        public int IdMovimiento { get; set; }
        public int IdArticulo { get; set; }
        public int IdTipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
        [StringLength(255)]
        public string Observaciones { get; set; }
        public Articulo Articulo { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
