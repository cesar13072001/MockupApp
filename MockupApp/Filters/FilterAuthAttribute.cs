using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Filters
{
    public class FilterAuthAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var usuario = (sp_LoginUsuario_Result)HttpContext.Current.Session["usuario"];
            if (usuario != null)
            {
                filterContext.Result = new RedirectResult("/");
                

            }
            
            
            base.OnActionExecuting(filterContext);
        }

    }
}