using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;
using System.Diagnostics;

namespace cicaudittrail.Controllers
{
    public class CicMessageTemplateController : Controller
    {
        private readonly ICicMessageTemplateRepository cicmessagetemplateRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

        // If you are using Dependency Injection, you can delete the following constructor
        public CicMessageTemplateController()
            : this(new CicMessageTemplateRepository())
        {
        }

        public CicMessageTemplateController(ICicMessageTemplateRepository cicmessagetemplateRepository)
        {
            this.cicmessagetemplateRepository = cicmessagetemplateRepository;
        }

        //
        // GET: /CicMessageTemplate/

        public ViewResult Index()
        {
            return View(cicmessagetemplateRepository.All);
        }

        //
        // GET: /CicMessageTemplate/Details/5

        public ViewResult Details(long id)
        {
            return View(cicmessagetemplateRepository.Find(id));
        }

        //
        // GET: /CicMessageTemplate/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CicMessageTemplate/Create

        [HttpPost]
        public ActionResult Create(CicMessageTemplate cicmessagetemplate)
        {
            //Debug.WriteLine("Request.Form = " + Request.Form);
            if (ModelState.IsValid)
            {
                cicmessagetemplate.DateCreated = DateTime.Now;
                cicmessagetemplate.UserCreated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                cicmessagetemplateRepository.InsertOrUpdate(cicmessagetemplate);
                cicmessagetemplateRepository.Save();

                TempData["message"] = " Modèle de message enregistré avec succés";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CicMessageTemplate/Edit/5

        public ActionResult Edit(long id)
        {
            return View(cicmessagetemplateRepository.Find(id));
        }

        //
        // POST: /CicMessageTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(CicMessageTemplate cicmessagetemplate)
        {
            if (ModelState.IsValid)
            {
                cicmessagetemplateRepository.InsertOrUpdate(cicmessagetemplate);
                cicmessagetemplateRepository.Save();

                TempData["message"] = " Modèle de message modifié avec succés";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CicMessageTemplate/Delete/5

        public ActionResult Delete(long id)
        {
            return View(cicmessagetemplateRepository.Find(id));
        }

        //
        // POST: /CicMessageTemplate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicmessagetemplateRepository.Delete(id);
            cicmessagetemplateRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cicmessagetemplateRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

