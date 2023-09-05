using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class LoginExtention : Controller
    {
        public LoginExtention()
        {

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Session["Username"] as string == null || Session["Username"] as string == "")
            {
                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Login")
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(new { action = "Index", Controller = "Login" }));
            }
        }

        public void saveUsuarioSessao(string Username, int id)
        {
            Session["Username"] = Username;
            Session["IdUsuario"] = id;
        }

        public void limparUsuarioSessao()
        {
            Session["Username"] = null;
            Session["IdUsuario"] = null;
        }
    }
}