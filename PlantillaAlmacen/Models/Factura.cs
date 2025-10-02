using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } = null!;
        public string ArchivoFactura { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
