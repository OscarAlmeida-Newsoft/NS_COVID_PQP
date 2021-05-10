using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class RespuestaFilterDTO
    {
        public string FiltroBusqueda { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }

        public string Sede { get; set; }
    }
}
