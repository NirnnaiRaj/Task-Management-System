using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Buisness.Filters
{
    public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {

        /// <summary>
        /// this filter is to authorize methods in controllers. If session contains username then will get that page, else redirects to login page
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {            
            string currentUser = Convert.ToString(context.HttpContext.Session.GetString("userEMail"));

            if (currentUser == null)
            {                
               context.Result = new RedirectToRouteResult
               (
                   new RouteValueDictionary(new
                   {
                        action = "Login",
                        controller = "User",
                        area =""
                   }));
                
              }
           
        }
    }
}
