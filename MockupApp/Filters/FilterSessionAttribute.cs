using MockupApp.Controllers;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MockupApp.Filters
{
    public class FilterSessionAttribute : ActionFilterAttribute
    {
        private int idRol;

        public FilterSessionAttribute(int idRol) {
            this.idRol = idRol;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var usuario = (sp_LoginUsuario_Result)HttpContext.Current.Session["usuario"];
            if (usuario != null)
            {

                if (usuario.idRol != 1)
                {
                    if (usuario.idRol != this.idRol)
                    {
                        filterContext.Result = new RedirectResult("/");

                    }
                }

            }
            else
            {
                filterContext.Result = new RedirectResult("/");
            }
             
            base.OnActionExecuting(filterContext);
        }

    }
}