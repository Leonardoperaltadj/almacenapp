using Microsoft.EntityFrameworkCore;

namespace PlantillaAlmacen.Models
{
    public class ArticuloCombustible : Articulo
    {
        public string Tipo { get; set; }
        [Precision(18, 2)]
        public decimal Cantidad { get; set; }
        public string Destino { get; set; }
        public DateTime FechaSalida { get; set; }
    }
}
