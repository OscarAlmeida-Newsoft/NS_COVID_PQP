using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class ObjRespuestaDTO
    {
        public int IdPersona { get; set; }
        public string Tipo { get; set; }
        public List<DetalleRespuesta> DetalleRespuestas { get; set; }
    }
}
