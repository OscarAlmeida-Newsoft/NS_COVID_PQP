using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NS_COVID_Entities.Entities;
using NS_COVID_Repositories;

namespace NS_COVID_Services
{
    public class PreguntaService
    {
        private PreguntaRepository preguntaRepository;
        private OpcionesRepository opcionesRepository;

        public PreguntaService() {
            preguntaRepository = new PreguntaRepository();
            opcionesRepository = new OpcionesRepository();
        }

        public List<Pregunta> getPreguntasActivas(string formulario)
        {
            List <Pregunta> preguntas = preguntaRepository.getPreguntasActivas(formulario);


            //Buscar las opciones
            foreach (Pregunta p in preguntas)
            {
                p.opciones = opcionesRepository.getOpcionesByPregunta(p.Id);
            }

            return preguntas;
        }
    }
}
