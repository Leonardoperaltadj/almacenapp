using System.ComponentModel.DataAnnotations;

namespace PlantillaAlmacen.Models.Auth
{
    public class LoginVM
    {
        [Required, EmailAddress, Display(Name = "Correo")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}
