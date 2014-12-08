using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace cicaudittrail.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var CurrentUser = System.Web.Security.Membership.GetUser();
            Session["CurrentUser"] = CurrentUser.UserName + "[" + CurrentUser.Email + "]";
            

            
            ViewBag.Message = "Modifiez ce modèle pour dynamiser votre application ASP.NET MVC.";

            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "Modifiez ce modèle pour dynamiser votre application ASP.NET MVC.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Votre page de description d’application.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Votre page de contact.";

            return View();
        }
    }
}
