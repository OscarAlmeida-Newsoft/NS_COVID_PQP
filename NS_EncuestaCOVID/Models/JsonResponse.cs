using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Models
{
    public class JsonResponse
    {
        public bool Error { get; set; }

        public bool GeneroAlerta { get; set; }

        public PersonaVM persona { get; set; }

        public string Mensaje { get; set; }
    }
}