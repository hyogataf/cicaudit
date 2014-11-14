using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{   
    public class CicMessageMailDocumentsController : Controller
    {
		private readonly ICicMessageMailRepository cicmessagemailRepository;
		private readonly ICicMessageMailDocumentsRepository cicmessagemaildocumentsRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CicMessageMailDocumentsController() : this(new CicMessageMailRepository(), new CicMessageMailDocumentsRepository())
        {
        }

        public CicMessageMailDocumentsController(ICicMessageMailRepository cicmessagemailRepository, ICicMessageMailDocumentsRepository cicmessagemaildocumentsRepository)
        {
			this.cicmessagemailRepository = cicmessagemailRepository;
			this.cicmessagemaildocumentsRepository = cicmessagemaildocumentsRepository;
        }

        //
        // GET: /CicMessageMailDocuments/

        public ViewResult Index()
        {
            return View(cicmessagemaildocumentsRepository.AllIncluding(cicmessagemaildocuments => cicmessagemaildocuments.CicMessageMail));
        }

        //
        // GET: /CicMessageMailDocuments/Details/5

        public ViewResult Details(long id)
        {
            return View(cicmessagemaildocumentsRepository.Find(id));
        }

        //
        // GET: /CicMessageMailDocuments/Create

        public ActionResult Create()
        {
			ViewBag.PossibleCicMessageMail = cicmessagemailRepository.All;
            return View();
        } 

        //
        // POST: /CicMessageMailDocuments/Create

        [HttpPost]
        public ActionResult Create(CicMessageMailDocuments cicmessagemaildocuments)
        {
            if (ModelState.IsValid) {
                cicmessagemaildocumentsRepository.InsertOrUpdate(cicmessagemaildocuments);
                cicmessagemaildocumentsRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicMessageMail = cicmessagemailRepository.All;
				return View();
			}
        }
        
        //
        // GET: /CicMessageMailDocuments/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleCicMessageMail = cicmessagemailRepository.All;
             return View(cicmessagemaildocumentsRepository.Find(id));
        }

        //
        // POST: /CicMessageMailDocuments/Edit/5

        [HttpPost]
        public ActionResult Edit(CicMessageMailDocuments cicmessagemaildocuments)
        {
            if (ModelState.IsValid) {
                cicmessagemaildocumentsRepository.InsertOrUpdate(cicmessagemaildocuments);
                cicmessagemaildocumentsRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicMessageMail = cicmessagemailRepository.All;
				return View();
			}
        }

        //
        // GET: /CicMessageMailDocuments/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(cicmessagemaildocumentsRepository.Find(id));
        }

        //
        // POST: /CicMessageMailDocuments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicmessagemaildocumentsRepository.Delete(id);
            cicmessagemaildocumentsRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                cicmessagemailRepository.Dispose();
                cicmessagemaildocumentsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

