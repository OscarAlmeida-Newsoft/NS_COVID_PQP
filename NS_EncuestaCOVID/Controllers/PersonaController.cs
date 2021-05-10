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
 
    public class PersonaController : Controller
    {
        private PersonaService personaService;
        private PreguntaService preguntaService;

        public PersonaController() {
            personaService = new PersonaService();
            preguntaService = new PreguntaService();
        }

        public ActionResult Index(string cedula = "")
        {
            PersonaBusquedaVM busqueda = new PersonaBusquedaVM();
            busqueda.NumeroDocumento = "";

            return View(busqueda);
        }

        [HttpPost]
        public ActionResult Index(PersonaBusquedaVM pModel)
        {

            if (ModelState.IsValid)
            {
                //Verificar si existe y redireccionar a pagina de ingreso de Encuesta
                Persona persona = personaService.getPersonaByCedula(pModel.NumeroDocumento);

                if (persona == null)
                {
                    return RedirectToAction("Registrar", new { cedula = pModel.NumeroDocumento });
                }
                else {
                    if (!persona.GeneroAlerta)
                    {
                        return RedirectToAction("Index", "Respuestas", new { cedula = persona.NumeroDocumento });

                    }
                    else
                    {
                        return RedirectToAction("Alerta");

                    }
                }
            }
            else {
                return View(pModel);
            }
        }

        public ActionResult Alerta()
        {
            return View();
        }

        public ActionResult Registrar(string cedula="") {
            PersonaVM laPersona = new PersonaVM();
            laPersona.TipoDocumento = "CC";

            if (!string.IsNullOrEmpty(cedula)) {

                //Verificar si existe y redireccionar a pagina de ingreso de Encuesta
                Persona persona = personaService.getPersonaByCedula(cedula);

                if (persona != null)
                {

                    return RedirectToAction("Index", "Respuestas", new { cedula = persona.NumeroDocumento });
                }
                else {
                    laPersona.NumeroDocumento = cedula;
                }

            }

            laPersona.cuestionario = preguntaService.getPreguntasActivas("Registro");

            //Lleno los select list
            //Tipos de Documento
            List<TipoDocumento> tipos = new List<TipoDocumento>();
            tipos.Add(new TipoDocumento() { valor = "CC", descripcion = "Cédula de Ciudadanía" });
            tipos.Add(new TipoDocumento() { valor = "TI", descripcion = "Tarjeta de Identidad" });
            tipos.Add(new TipoDocumento() { valor = "CE", descripcion = "Cédula de Extranjería" });
            tipos.Add(new TipoDocumento() { valor = "PE", descripcion = "Permiso especial" });
            tipos.Add(new TipoDocumento() { valor = "PP", descripcion = "Pasaporte" });

            laPersona.TipoDocumentoList = new SelectList(tipos, "valor", "descripcion");

            //Vinculo
            List<Vinculo> vinculos = new List<Vinculo>();
            vinculos.Add(new Vinculo() { Codigo = "", Descripcion = "Seleccione" });
            vinculos.Add(new Vinculo() { Codigo = "Empleado", Descripcion = "Empleado" });
            vinculos.Add(new Vinculo() { Codigo = "Visitante", Descripcion = "Visitante" });

            laPersona.Vinculos = new SelectList(vinculos, "Codigo", "Descripcion");

            return View(laPersona);
        }

        [HttpPost]
        public JsonResult Registrar(PersonaVM pModel) {

                //Obtiene lista de preguntas
                pModel.cuestionario = preguntaService.getPreguntasActivas("Registro");

                //Validar si el registro ya existe
                Persona persona = personaService.getPersonaByCedula(pModel.NumeroDocumento);
                Persona laPersona = null;

                if (persona == null) {
                    pModel.Nombres = pModel.Nombres.ToUpper();
                    pModel.Apellidos = pModel.Apellidos.ToUpper();
                    //Crear el registro para la persona
                    Persona nuevaPersona = Mapper.Map<PersonaVM, Persona>(pModel);
                    laPersona = personaService.createPersona(nuevaPersona);
                }
            //Convierte el model en viewModel
            PersonaVM personaVm = new PersonaVM();
            if (laPersona != null)
            {
                personaVm.Nombres = $"{laPersona.Nombres} {laPersona.Apellidos}";
                personaVm.cadenaPregunta = laPersona.CadenaPreguntas;
                personaVm.TipoDocumento = laPersona.TipoDocumento;
                personaVm.NumeroDocumento = laPersona.NumeroDocumento;
                personaVm.FechaHoraCreacion = laPersona.FechaHoraCreacion;
            }

            return Json(new JsonResponse() { Error = false, GeneroAlerta = laPersona.GeneroAlerta, persona = personaVm });


        }

        //[HttpPost]
        //public JsonResult EnviarRespuestas(ObjRespuestaVM response)
        //{

        //    //Guarda respuestas enviadas por el usuario
        //    respuestaService.GuardarRespuestas(Mapper.Map<ObjRespuestaVM, ObjRespuestaDTO>(response));

        //    return Json(new JsonResponse() { Error = false });
        //}

        public FilePathResult DescargaTerminosUso() {

            string path = Server.MapPath("~/App_Data/TermsAndConditions.pdf");

            return new FilePathResult(path, "application/pdf");
        }
    }
}