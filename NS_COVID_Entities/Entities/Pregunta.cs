using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Entities.Entities
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string TipoValor { get; set; }
        public string Enunciado { get; set; }
        public string ValorXDefecto { get; set; }
        public bool GeneraAlerta { get; set; }
        public string CondicionGeneraAlerta { get; set; }
        public string ValorGeneraAlerta { get; set; }
        public int Orden { get; set; }
        public List<Opciones> opciones { get; set; }

    }
}
