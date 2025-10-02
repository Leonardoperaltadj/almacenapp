namespace PlantillaAlmacen.Models
{
    public class ArticuloEnfermeria : Articulo
    {
        public DateTime FechaCaducidad { get; set; }
        public string Lote { get; set; }
    }
}
