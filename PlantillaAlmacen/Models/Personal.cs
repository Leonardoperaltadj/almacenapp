using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaAlmacen.Models
{
    public class Personal
    {
        [Key]
        public int IdPersonal { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Display(Name = "Puesto")]
        public int IdPuesto { get; set; }

        [Required]
        public int IdDepartamento { get; set; }
        [ForeignKey("IdDepartamento")]
        [ValidateNever]
        public Departamento Departamento { get; set; }

        [Required]
        public int IdEstatusPersonal { get; set; }
        [ForeignKey("IdEstatusPersonal")]
        [ValidateNever]
        public EstatusPersonal EstatusPersonal { get; set; }

        [Display(Name = "Frente")]
        public int IdFrente { get; set; }
        [ForeignKey("IdFrente")]
        [ValidateNever]
        public Frente Frente { get; set; }
        [ForeignKey("IdPuesto")]
        [ValidateNever]
        public Puesto Puesto { get; set; }
        [Display(Name = "¿Tiene licencia de manejo?")]
        public bool TieneLicenciaManejo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Vigencia licencia")]
        public DateTime? VigenciaLicencia { get; set; }

        [StringLength(255)]
        [Display(Name = "Archivo licencia")]
        public string? ArchivoLicencia { get; set; }
    }
}
