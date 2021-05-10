using NS_COVID_Entities;
using NS_COVID_Entities.Entities;
using NS_COVID_Repositories;
using NS_COVID_Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Services
{
    public class PersonaService
    {
        private PersonaRepository personaRepository;
        private PreguntaRepository preguntaRepository;
        private CorreoRepository correoRepository;
        private DetalleRespuestaRepository detalleRespuestaRepository;
        private CorrreoService corrreoService;

        public PersonaService() {
            correoRepository = new CorreoRepository();
            personaRepository = new PersonaRepository();
            preguntaRepository = new PreguntaRepository();
            detalleRespuestaRepository = new DetalleRespuestaRepository();
            corrreoService = new CorrreoService();
        }

        public Persona getPersonaByCedula(string cedula) {

            Persona laPersona = personaRepository.getPersonaByCedula(cedula);

            return laPersona;
        }

        public Persona createPersona(Persona laPersona) {

            List<Correo> correosNotificar = null;
            string cadenaPreguntas = string.Empty;
            if (laPersona.respuestas != null)
            {

                //Evaluar si se levantan alertas
                List<Pregunta> preguntas = preguntaRepository.getPreguntasActivas("Registro");

                foreach (DetalleRespuesta p in laPersona.respuestas)
                {
                    //Validar cada una de las respuestas
                    Pregunta miPregunta = preguntas.SingleOrDefault(d => d.Id == p.IdPregunta);
                    p.Pregunta = miPregunta;

                    if (miPregunta.GeneraAlerta)
                    {
                        
                        //Evalua los distintos tipos de valores
                        switch (miPregunta.TipoValor)
                        {
                            case "texto":

                                //Evalua los distintos tipos de condiciones para texto
                                switch (miPregunta.CondicionGeneraAlerta)
                                {
                                    case "=":
                                        if (p.ValorRespuesta == miPregunta.ValorGeneraAlerta)
                                        {
                                            laPersona.GeneroAlerta = true;
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
                                            laPersona.GeneroAlerta = true;
                                            p.GeneroAlerta = true;
                                        }
                                        break;
                                }

                                break;
                        }
                    }
                }
                
                if (laPersona.GeneroAlerta)
                {
                    //Busca las personas a notificar
                    correosNotificar = correoRepository.getCorreosActivos();
                    string textoCorreos = "";

                    if (correosNotificar.Count > 0)
                    {
                        foreach (Correo c in correosNotificar)
                        {
                            textoCorreos += c.CorreoElectronico + ",";
                        }

                        textoCorreos = textoCorreos.Substring(0, textoCorreos.Length - 1);

                        //laPersona.PersonasNotificadas = textoCorreos;
                    }
                }
            }
            int idPersona = personaRepository.createPersona(laPersona);
            if (laPersona.respuestas != null)
            {
                //Guarda Detalle
                detalleRespuestaRepository.GuardarDetalleRespuestas(laPersona.respuestas, null, idPersona);
                laPersona.FechaHoraCreacion = DateTime.Now.ToString();
                //TODO: Armar logica para enviar correo en caso de alerta
                if (laPersona.GeneroAlerta)
                {
                    //Envio correo de alertas
                    Persona miPersona = personaRepository.getPersonaById(idPersona);
                    laPersona.FechaHoraCreacion = miPersona.FechaHoraCreacion;
                    string Asunto = "Registro de COVID de " + miPersona.Nombres + " " + miPersona.Apellidos;
                    string Body = "<h3>" + miPersona.Nombres + " " + miPersona.Apellidos +
                        " presenta sintomas de alarma en las siguientes preguntas:</h3>";
                    foreach (DetalleRespuesta d in laPersona.respuestas)
                    {
                        if (d.ValorRespuesta == Constants.respuestas.respuestSi)
                        {
                            cadenaPreguntas += $"{d.Pregunta.Enunciado} </br>";
                        }
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
                }
            }
            laPersona.CadenaPreguntas = cadenaPreguntas;
            return laPersona;
        }

        /// <summary>
        /// Se consulta los accesos a los que se tiene acceso.
        /// </summary>
        /// <param name="userName">Usuario de la persona que esta usando la aplicación</param>
        /// <returns></returns>
        public string GetPermissionsByUsers(string userName)
        {
            string menu = $"<a href='/Persona/Index'>Registrar temperatura</a>"; ;
            List<Accesos> accesPerUser = personaRepository.GetPermissionsByUsers(userName);
            foreach (var item in accesPerUser)
            {
                menu += $"<a  href='{item.Rutas}'>{item.Nombre}</a>";
            }
            return menu;
        }
    }
}
