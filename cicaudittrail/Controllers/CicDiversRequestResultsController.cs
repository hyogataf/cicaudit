using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{   
    public class CicDiversRequestResultsController : Controller
    {
		private readonly ICicDiversRequestResultsRepository cicdiversrequestresultsRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CicDiversRequestResultsController() : this(new CicDiversRequestResultsRepository())
        {
        }

        public CicDiversRequestResultsController(ICicDiversRequestResultsRepository cicdiversrequestresultsRepository)
        {
			this.cicdiversrequestresultsRepository = cicdiversrequestresultsRepository;
        }

        //
        // GET: /CicDiversRequestResults/

        public ViewResult Index()
        {
            return View(cicdiversrequestresultsRepository.All);
        }

        //
        // GET: /CicDiversRequestResults/Details/5

        public ViewResult Details(long id)
        {
            return View(cicdiversrequestresultsRepository.Find(id));
        }

        //
        // GET: /CicDiversRequestResults/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CicDiversRequestResults/Create

        [HttpPost]
        public ActionResult Create(CicDiversRequestResults cicdiversrequestresults)
        {
            if (ModelState.IsValid) {
                cicdiversrequestresultsRepository.InsertOrUpdate(cicdiversrequestresults);
                cicdiversrequestresultsRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CicDiversRequestResults/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(cicdiversrequestresultsRepository.Find(id));
        }

        //
        // POST: /CicDiversRequestResults/Edit/5

        [HttpPost]
        public ActionResult Edit(CicDiversRequestResults cicdiversrequestresults)
        {
            if (ModelState.IsValid) {
                cicdiversrequestresultsRepository.InsertOrUpdate(cicdiversrequestresults);
                cicdiversrequestresultsRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CicDiversRequestResults/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(cicdiversrequestresultsRepository.Find(id));
        }

        //
        // POST: /CicDiversRequestResults/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicdiversrequestresultsRepository.Delete(id);
            cicdiversrequestresultsRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                cicdiversrequestresultsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

