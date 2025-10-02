using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public bool Estatus { get; set; } = true;

    }
}
