using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class DetalleInformePersona
    {
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public DateTime? InicioJornanda { get; set; }
        public DateTime? FinJornada { get; set; }
        public string Sintomas { get; set; }
        public string TemperaturaInicio { get; set; }
        public string TemperaturaFin { get; set; }

        public int IdRespuestaInicio { get; set; }
        public int IdRespuestaFin { get; set; }
        public bool GeneroAlerta { get; set; }
        public bool GeneroAlertaInicio { get; set; }
        public bool GeneroAlertaFin { get; set; }

        
        
    }
}
