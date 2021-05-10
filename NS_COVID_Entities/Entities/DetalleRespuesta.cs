using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class DetalleRespuesta
    {
        public int IdPregunta { get; set; }
        public Pregunta Pregunta { get; set; }
        public string ValorRespuesta { get; set; }
        public bool GeneroAlerta { get; set; }
    }
}
