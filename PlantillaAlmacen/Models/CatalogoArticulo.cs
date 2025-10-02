using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class CatalogoArticulo
    {
        [Key]
        public int IdCatalogoArticulo { get; set; }
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "Debe seleccionar una categoría")]

        public int IdCategoria { get; set; }
        [StringLength(100)]
        [Required]
        public string Nombre { get; set; }
        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
        public bool Estatus { get; set; } = true;
        [Display(Name = "Unidad de medida")]
        public int? IdUnidadMedida { get; set; }
        [ForeignKey(nameof(IdUnidadMedida))]
        [ValidateNever]
        public UnidadMedida? UnidadMedida { get; set; }

        [ForeignKey("IdCategoria")]
        [ValidateNever]
        public Categoria Categoria { get; set; }

    }
}
