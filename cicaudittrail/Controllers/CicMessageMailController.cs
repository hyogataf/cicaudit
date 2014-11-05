using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{   
    public class CicMessageMailController : Controller
    {
		private readonly ICicRequestResultsFollowedRepository cicrequestresultsfollowedRepository;
		private readonly ICicMessageMailRepository cicmessagemailRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CicMessageMailController() : this(new CicRequestResultsFollowedRepository(), new CicMessageMailRepository())
        {
        }

        public CicMessageMailController(ICicRequestResultsFollowedRepository cicrequestresultsfollowedRepository, ICicMessageMailRepository cicmessagemailRepository)
        {
			this.cicrequestresultsfollowedRepository = cicrequestresultsfollowedRepository;
			this.cicmessagemailRepository = cicmessagemailRepository;
        }

        //
        // GET: /CicMessageMail/

        public ViewResult Index()
        {
            return View(cicmessagemailRepository.AllIncluding(cicmessagemail => cicmessagemail.CicRequestResultsFollowed));
        }

        //
        // GET: /CicMessageMail/Details/5

        public ViewResult Details(long id)
        {
            return View(cicmessagemailRepository.Find(id));
        }

        //
        // GET: /CicMessageMail/Create

        public ActionResult Create()
        {
			ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
            return View();
        } 

        //
        // POST: /CicMessageMail/Create

        [HttpPost]
        public ActionResult Create(CicMessageMail cicmessagemail)
        {
            if (ModelState.IsValid) {
                cicmessagemailRepository.InsertOrUpdate(cicmessagemail);
                cicmessagemailRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
				return View();
			}
        }
        
        //
        // GET: /CicMessageMail/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
             return View(cicmessagemailRepository.Find(id));
        }

        //
        // POST: /CicMessageMail/Edit/5

        [HttpPost]
        public ActionResult Edit(CicMessageMail cicmessagemail)
        {
            if (ModelState.IsValid) {
                cicmessagemailRepository.InsertOrUpdate(cicmessagemail);
                cicmessagemailRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
				return View();
			}
        }

        //
        // GET: /CicMessageMail/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(cicmessagemailRepository.Find(id));
        }

        //
        // POST: /CicMessageMail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicmessagemailRepository.Delete(id);
            cicmessagemailRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                cicrequestresultsfollowedRepository.Dispose();
                cicmessagemailRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

