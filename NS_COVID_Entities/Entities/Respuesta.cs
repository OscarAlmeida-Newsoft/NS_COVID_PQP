using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class Respuesta
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int IdPersona { get; set; }
        public bool GeneroAlerta { get; set; }
        public string PersonasNotificadas { get; set; }
    }
}
