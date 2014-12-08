using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace cicaudittrail.Filters
{
    public sealed class LogonAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            Trace.WriteLine(" skipAuthorization  = " + skipAuthorization);
            if (!skipAuthorization)
            {
                Trace.WriteLine(" currentUser  = " );
                base.OnAuthorization(filterContext);
            }
        }
    }
}