using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NS_EncuestaCOVID.BusinessRules
{
    public class CustomAuthorize : AuthorizeAttribute
    {

    
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }

          
        }
    }
}