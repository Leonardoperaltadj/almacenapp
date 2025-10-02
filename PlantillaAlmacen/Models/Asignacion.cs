using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class Asignacion
    {
        [Key]
        public int IdAsignacion { get; set; }
        public int IdArticulo { get; set; }
        public int? IdPersonal { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdFrente { get; set; }
        public int IdEstadoEntrega { get; set; }
        public int? IdEstadoRecepcion { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de asignación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAsignacion { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        [StringLength(255)]
        public string? ObservacionesDevolucion { get; set; }

        public bool Activo { get; set; } = true;
        public string? ArchivoAsignacion { get; set; }
        public string? ArchivoDanios { get; set; }
        [ForeignKey(nameof(IdArticulo))][ValidateNever] public Articulo Articulo { get; set; }
        [ForeignKey(nameof(IdPersonal))][ValidateNever] public Personal? Personal { get; set; }
        [ForeignKey(nameof(IdDepartamento))][ValidateNever] public Departamento? Departamento { get; set; }
        [ForeignKey(nameof(IdFrente))][ValidateNever] public Frente? Frente { get; set; }
        [ForeignKey(nameof(IdEstadoEntrega))][ValidateNever] public EstadoEntrega EstadoEntrega { get; set; }
        [ForeignKey(nameof(IdEstadoRecepcion))][ValidateNever] public EstadoEntrega? EstadoRecepcion { get; set; }
        public ICollection<AsignacionEvento> Eventos { get; set; } = new List<AsignacionEvento>();
        public ICollection<EvidenciaAsignacion> Evidencias { get; set; } = new List<EvidenciaAsignacion>();

    }

}
