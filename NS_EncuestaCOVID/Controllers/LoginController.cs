using NS_COVID_Services;
using NS_EncuestaCOVID.BusinessRules;
using NS_EncuestaCOVID.Constants;
using NS_EncuestaCOVID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NS_EncuestaCOVID.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session["Login"] = false;
            Session["User"] = "";
            return View();
        }

        private bool validarLogIn()
        {
            try
            {
                var status = (bool)Session["Login"];
                string user = Session["User"].ToString();

                if (!status && user == "")
                {
                    return false;
                }
                return true;
            }
            catch
            {
                Session["Login"] = false;
                Session["User"] = "";
                return false;
               
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ValidateUser()
        {
            if (!validarLogIn())
            {
                return Json(new JsonResponse() { Error = false, Mensaje =  "<a href='/Persona/Index'>Registrar temperatura</a>" });
            }
            PersonaService user = new PersonaService();
            var result = user.GetPermissionsByUsers(Session["User"].ToString());
            return Json(new JsonResponse() {Mensaje = result,Error = true });
        }
        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginVM model, string url)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserService userService = new UserService();
            var result = userService.SingIn(model.Email.Trim(), model.Password.Trim(), true);
            switch (result)
            {
                case Enums.SignInStatus.Success:
                    Session["Login"] = true;
                    Session["User"] = model.Email.Trim();
                    return RedirectToAction("Index", "Persona");

                case Enums.SignInStatus.LockedOut:
                    return RedirectToAction("Index", "Login");


                case Enums.SignInStatus.RequiresVerification:
                    return RedirectToAction("Index", "Login");


                case Enums.SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Valide sus datos e inténtelo de nuevo.");
                    return View("Index");
            }
        }
    }
}