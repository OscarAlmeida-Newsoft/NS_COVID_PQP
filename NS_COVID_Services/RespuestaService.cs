using NS_COVID_Entities;
using NS_COVID_Entities.Entities;
using NS_COVID_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Services
{
    public class RespuestaService
    {
        private RespuestaRepository respuestaRepository;
        private PreguntaRepository preguntaRepository;
        private CorreoRepository correoRepository;
        private DetalleRespuestaRepository detalleRespuestaRepository;
        private PersonaRepository personaRepository;
        private CorrreoService corrreoService;

        public RespuestaService() {
            respuestaRepository = new RespuestaRepository();
            preguntaRepository = new PreguntaRepository();
            correoRepository = new CorreoRepository();
            detalleRespuestaRepository = new DetalleRespuestaRepository();
            personaRepository = new PersonaRepository();
            corrreoService = new CorrreoService();
        }

        public List<Respuesta> getRespuestasXUsuarioXDia(int IdPersona) {

            return respuestaRepository.getRespuestasXUsuarioXDia(IdPersona);
        }

        public bool GuardarRespuestas(ObjRespuestaDTO objRespuestaDTO)
        {
            //Comienzo armando el objeto
            Respuesta miRespuesta = new Respuesta();
            miRespuesta.IdPersona = objRespuestaDTO.IdPersona;
            miRespuesta.Tipo = objRespuestaDTO.Tipo;
            miRespuesta.GeneroAlerta = false;
            List<Correo> correosNotificar = null;

            //Evaluar si se levantan alertas
            List<Pregunta> preguntas = preguntaRepository.getPreguntasActivas("Cuestionario");

            foreach (DetalleRespuesta p in objRespuestaDTO.DetalleRespuestas) {
                //Validar cada una de las respuestas
                Pregunta miPregunta = preguntas.SingleOrDefault(d => d.Id == p.IdPregunta);
                p.Pregunta = miPregunta;

                if (miPregunta.GeneraAlerta) {

                    //Evalua los distintos tipos de valores
                    switch (miPregunta.TipoValor) {
                        case "texto":

                            //Evalua los distintos tipos de condiciones para texto
                            switch (miPregunta.CondicionGeneraAlerta) {
                                case "=":
                                    if (p.ValorRespuesta == miPregunta.ValorGeneraAlerta) {
                                        miRespuesta.GeneroAlerta = true;
                                        p.GeneroAlerta = true;
                                    }
                                    break;
                            }

                            break;
                        case "decimal":

                            //Evalua los distintos tipos de condiciones para decimal
                            switch (miPregunta.CondicionGeneraAlerta)
                            {
                                case ">=":
                                    if (Convert.ToDecimal(p.ValorRespuesta) >= Convert.ToDecimal(miPregunta.ValorGeneraAlerta))
                                    {
                                        miRespuesta.GeneroAlerta = true;
                                        p.GeneroAlerta = true;
                                    }
                                    break;
                            }

                            break;
                    }
                }
            }

            if (miRespuesta.GeneroAlerta) {
                //Busca las personas a notificar
                correosNotificar = correoRepository.getCorreosActivos();
                string textoCorreos = "";

                if (correosNotificar.Count > 0) {
                    foreach (Correo c in correosNotificar)
                    {
                        textoCorreos += c.CorreoElectronico + ",";
                    }

                    textoCorreos = textoCorreos.Substring(0, textoCorreos.Length - 1);

                    miRespuesta.PersonasNotificadas = textoCorreos;
                }

                
            }

            //Guarda encabezado
            int IdRespuesta = respuestaRepository.GuardarRespuesta(miRespuesta);

            if (IdRespuesta != 0)
            {
                //Guarda Detalle
                detalleRespuestaRepository.GuardarDetalleRespuestas(objRespuestaDTO.DetalleRespuestas, IdRespuesta, null);

                if (miRespuesta.GeneroAlerta)
                {
                    //Envio correo de alertas
                    Persona miPersona = personaRepository.getPersonaById(miRespuesta.IdPersona);


                    string Asunto = "Registro de COVID de " + miPersona.Nombres + " " + miPersona.Apellidos;
                    string Body = "<h3>" + miPersona.Nombres + " " + miPersona.Apellidos +
                        " presenta sintomas de alarma en las siguientes preguntas:</h3>";

                    foreach (DetalleRespuesta d in objRespuestaDTO.DetalleRespuestas)
                    {

                        Body += "<p>" +
                                    "<div>Pregunta: " + d.Pregunta.Enunciado + "</div>" +
                                    "<div>Respuesta: " + d.ValorRespuesta + "</div>" +
                                "</p>";
                    }

                    List<string> correos = new List<string>();
                    foreach (Correo c in correosNotificar)
                    {
                        correos.Add(c.CorreoElectronico);
                    }

                    corrreoService.EnviaCorreo(correos, Asunto, Body);

                    if (miPersona.Vinculo == "Visitante")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
            
        }

        public string GetSedes()
        {
            return respuestaRepository.GetSedes();
        }

        public List<DetalleInformePersona> GetInformeGeneral(RespuestaFilterDTO respuestaFilterDTO)
        {

            List<DetalleInformePersona> informe = respuestaRepository.GetInformeGeneral(respuestaFilterDTO);


            foreach (DetalleInformePersona item in informe)
            {

                if (item.GeneroAlertaInicio || item.GeneroAlertaFin)
                {
                    item.GeneroAlerta = true;
                    List<string> Sintomas = new List<string>();

                    if (item.GeneroAlertaInicio)
                    {
                        //Busca detalle respuestas del inicio
                        List<DetalleRespuesta> det = detalleRespuestaRepository.getDetalleRespustasPorId(item.IdRespuestaInicio);

                        foreach (DetalleRespuesta d in det)
                        {
                            if (d.GeneroAlerta)
                            {
                                if (!Sintomas.Exists(f => f == d.Pregunta.Descripcion))
                                {
                                    Sintomas.Add(d.Pregunta.Descripcion);
                                }
                            }
                        }
                    }

                    if (item.GeneroAlertaFin)
                    {
                        //Busca detalle en respuestas de fin
                        if (item.IdRespuestaFin != 0)
                        {

                            List<DetalleRespuesta> det = detalleRespuestaRepository.getDetalleRespustasPorId(item.IdRespuestaFin);

                            foreach (DetalleRespuesta d in det)
                            {
                                if (d.GeneroAlerta)
                                {
                                    if (!Sintomas.Exists(f => f == d.Pregunta.Descripcion))
                                    {
                                        Sintomas.Add(d.Pregunta.Descripcion);
                                    }
                                }
                            }
                        }
                    }

                    //Finalmente armo string a mostrar en la vista
                    foreach (string s in Sintomas)
                    {
                        item.Sintomas += s + ", ";
                    }

                    item.Sintomas = item.Sintomas.Substring(0, item.Sintomas.Length - 2);
                }
            }

            return informe;
        }

    }
}
