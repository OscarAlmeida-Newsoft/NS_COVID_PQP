using NS_COVID_Entities;
using NS_COVID_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Models
{
    public class RespuestaVM
    {
        public Persona persona { get; set; }
        public Respuesta inicioJornada { get; set; }
        public Respuesta finJornada { get; set; }
        public List<Pregunta> cuestionario { get; set; }
    }
}