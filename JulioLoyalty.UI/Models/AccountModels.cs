using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JulioLoyalty.UI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }

    public class RecoverModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Correo electrónico invalido")]
        public string Email { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La nueva contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación es obligatorio")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Token invalido")]
        public string Token { get; set; }
    }


    public class ChangePassword
    {
        [Required(ErrorMessage = "No. de membresía es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nueva contraseña es requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La contraseña de confirmación es requerido")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden")]
        public string ConfirmPassword { get; set; }
    }


}