using SolutionWebCadastroLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWebCadastroLogin.Controllers
{
    public class LoginExtention : Controller
    {
        public LoginExtention()
        {

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //base.OnActionExecuted(filterContext);

            //if (false && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Login")
            //{

            //    filterContext.Result = new RedirectToRouteResult(
            //        new System.Web.Routing.RouteValueDictionary(new { action = "Index", Controller = "Login" }));
            //}            
        }
    }
}