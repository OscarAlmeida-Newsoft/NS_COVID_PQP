using NS_COVID_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Models
{
    public class ObjRespuestaVM
    {
        public int IdPersona { get; set; }
        public string Tipo { get; set; }
        public List<DetalleRespuesta> DetalleRespuestas { get; set; }
    }
}