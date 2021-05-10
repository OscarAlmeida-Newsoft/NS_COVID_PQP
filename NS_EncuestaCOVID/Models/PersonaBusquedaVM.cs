using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Models
{
    public class PersonaBusquedaVM
    {
        [DisplayName("Documento")]
        [Required(ErrorMessage = "Debe ingresar el número de documento")]
        public string NumeroDocumento { get; set; }

        public bool GeneroAlerta { get; set; }
    }
}