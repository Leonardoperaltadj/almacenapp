using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PlantillaAlmacen.Models
{
    public class AsignacionEvento
    {
        [Key] public int IdEvento { get; set; }
        public int IdAsignacion { get; set; }
        [Required, StringLength(40)]
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int? IdUsuario { get; set; }
        [StringLength(500)] public string? Comentario { get; set; }

        public int? IdEstadoEntrega { get; set; }

        [ForeignKey(nameof(IdAsignacion))]
        [ValidateNever]

        public Asignacion Asignacion { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        [ValidateNever]

        public Usuario? Usuario { get; set; }
        [ForeignKey(nameof(IdEstadoEntrega))]
        [ValidateNever]

        public EstadoEntrega? EstadoEntrega { get; set; }
    }
}
