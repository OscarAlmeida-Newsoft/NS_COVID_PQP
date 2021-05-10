using AutoMapper;
using NS_COVID_Entities;
using NS_COVID_Entities.Entities;
using NS_COVID_Services;
using NS_EncuestaCOVID.BusinessRules;
using NS_EncuestaCOVID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NS_EncuestaCOVID.Controllers
{
    public class RespuestasController : Controller
    {
        private PersonaService personaService;
        private RespuestaService respuestaService;
        private PreguntaService preguntaService;

        public RespuestasController() {
            personaService = new PersonaService();
            respuestaService = new RespuestaService();
            preguntaService = new PreguntaService();
        }

        private bool validarLogIn()
        {
            try
            {
                var status = (bool)Session["Login"];
                if (!status)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult Index(string cedula)
        {
            if (!validarLogIn())
            {
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrEmpty(cedula)) {
                return RedirectToAction("Index", "Persona");
            }

            //Objeto a retornar a la vista
            RespuestaVM model = new RespuestaVM();
            

            //Valida si el empleado ya esta registrado
            Persona persona = personaService.getPersonaByCedula(cedula);

            if (persona == null) {
                return RedirectToAction("Registrar","Persona", new { cedula = cedula });
            }
            model.persona = persona;

            //Se verifica si ya se tienen respuestas
            List<Respuesta> respuestaUsuario = respuestaService.getRespuestasXUsuarioXDia(persona.Id);

            model.inicioJornada = respuestaUsuario.FirstOrDefault(d => d.Tipo == "I");
            model.finJornada = respuestaUsuario.FirstOrDefault(d => d.Tipo == "F");

            //Obtiene lista de preguntas
            model.cuestionario = preguntaService.getPreguntasActivas("Cuestionario");

            return View(model);
        }

        [HttpPost]
        public JsonResult EnviarRespuestas(ObjRespuestaVM response) {

            //Guarda respuestas enviadas por el usuario
            bool generoAlerta = respuestaService.GuardarRespuestas(Mapper.Map<ObjRespuestaVM, ObjRespuestaDTO>(response));

            return Json(new JsonResponse() { Error =false, GeneroAlerta = generoAlerta});
        }

        [HttpPost]
        public JsonResult SearchSede()
        {
            string generoAlerta = respuestaService.GetSedes();

            return Json(new JsonResponse() { Error = true, Mensaje = generoAlerta }) ;
        }

        public ActionResult RegistroExitoso() {

            return View();
        }

        public ActionResult AdminHome()
        {
            if (!validarLogIn())
            {
                return RedirectToAction("Index", "Login");
            }
            RespuestaFilterVM filtro = new RespuestaFilterVM();

            filtro.FechaInicial = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            filtro.FechaFinal = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            return View(filtro);
        }




        [HttpPost]
        public ActionResult List(RespuestaFilterVM filters)
        {

            List<DetalleInformePersona> informe = respuestaService.GetInformeGeneral(Mapper.Map<RespuestaFilterVM, RespuestaFilterDTO>(filters));

            return PartialView("_Grid", informe);
        }
    }
}
