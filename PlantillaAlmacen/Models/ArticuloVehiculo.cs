using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models
{
    public class ArticuloVehiculo : Articulo
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Placas { get; set; }
        public string TarjetaCirculacion { get; set; }
        public string PolizaSeguro { get; set; }

        public string Color { get; set; }
        public int Anio { get; set; }
        public string Combustible { get; set; }
        public int? Kilometraje { get; set; }

        [Required]
        [RegularExpression("^(Manual|Automática|Automatica)$",
            ErrorMessage = "La transmisión debe ser Manual o Automática")]
        public string Transmision { get; set; }
    }
}
