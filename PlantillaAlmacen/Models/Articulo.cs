using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    [Index(nameof(NumeroInventario), IsUnique = true)]

    public abstract class Articulo
    {
        [Key]
        public int IdArticulo { get; set; }
        public int IdCatalogoArticulo { get; set; }
        public int IdEstatusArticulo { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime FechaCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public int? IdFactura { get; set; }
        [ForeignKey("IdFactura")]
        public Factura? Factura { get; set; }

        [StringLength(255)]
        public string? Observacion { get; set; }

        [ForeignKey("IdCatalogoArticulo")]
        public CatalogoArticulo CatalogoArticulo { get; set; }

        [ForeignKey("IdEstatusArticulo")]
        public EstatusArticulo EstatusArticulo { get; set; }

        public int? IdAlmacen { get; set; }

        [ForeignKey("IdAlmacen")]
        public Almacen? Almacen { get; set; }
        public string NumeroInventario { get; set; }

        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();

    }

}
