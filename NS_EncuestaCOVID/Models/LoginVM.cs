using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Campo usuario es obligatorio")]
        [Display(Name = "Usuario:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo contraseña es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña:")]
        public string Password { get; set; }

        [Display(Name= "Recordar?")]
        public bool RemenberMe{ get; set; }

    }
}