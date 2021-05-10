using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NS_EncuestaCOVID.Models
{
    public class RespuestaFilterVM
    {
        public string FiltroBusqueda { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }

        public string Sede { get; set; }
    }
}