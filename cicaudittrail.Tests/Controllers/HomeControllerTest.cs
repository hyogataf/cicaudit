using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cicaudittrail;
using cicaudittrail.Controllers;

namespace cicaudittrail.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Réorganiser
            HomeController controller = new HomeController();

            // Agir
            ViewResult result = controller.Index() as ViewResult;

            // Déclarer
            Assert.AreEqual("Modifiez ce modèle pour dynamiser votre application ASP.NET MVC.", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Réorganiser
            HomeController controller = new HomeController();

            // Agir
            ViewResult result = controller.About() as ViewResult;

            // Déclarer
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Réorganiser
            HomeController controller = new HomeController();

            // Agir
            ViewResult result = controller.Contact() as ViewResult;

            // Déclarer
            Assert.IsNotNull(result);
        }
    }
}
