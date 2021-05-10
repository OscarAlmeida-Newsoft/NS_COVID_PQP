using NS_COVID_Entities.Entities;
using System;
using System.Collections.Generic;

namespace NS_COVID_Entities
{
    public class Persona
    {
        public int Id { get; set; }


        public string TipoDocumento { get; set; }


        public string NumeroDocumento { get; set; }


        public string Nombres { get; set; }


        public string Apellidos { get; set; }


        public string Sexo { get; set; }


        public DateTime FechaNacimiento { get; set; }


        public string NumeroCelular { get; set; }


        public string CorreoElectronico { get; set; }

        public string Vinculo { get; set; }

        public string Empresa { get; set; }

        public string Placa { get; set; }

        public bool AceptaTerminosUso { get; set; }

        public bool GeneroAlerta { get; set; }

        public string FechaHoraCreacion { get; set; }

        public string CadenaPreguntas { get; set; }
        public List<DetalleRespuesta> respuestas { get; set; }


    }
}
