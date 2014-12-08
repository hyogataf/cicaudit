using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class CicRequestExecutionController : Controller
    {
		private readonly ICicRequestRepository CicrequestRepository;
		private readonly ICicRequestExecutionRepository CicrequestexecutionRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CicRequestExecutionController() : this(new CicRequestRepository(), new CicRequestExecutionRepository())
        {
        }

        public CicRequestExecutionController(ICicRequestRepository CicrequestRepository, ICicRequestExecutionRepository CicrequestexecutionRepository)
        {
			this.CicrequestRepository = CicrequestRepository;
			this.CicrequestexecutionRepository = CicrequestexecutionRepository;
        }

        //
        // GET: /CicRequestExecution/

        public ViewResult Index()
        {
            return View(CicrequestexecutionRepository.AllIncluding(Cicrequestexecution => Cicrequestexecution.CicRequest));
        }

        //
        // GET: /CicRequestExecution/Details/5

        public ViewResult Details(long id)
        {
            return View(CicrequestexecutionRepository.Find(id));
        }

        //
        // GET: /CicRequestExecution/Create

        public ActionResult Create()
        {
			ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View();
        } 

        //
        // POST: /CicRequestExecution/Create

        [HttpPost]
        public ActionResult Create(CicRequestExecution Cicrequestexecution)
        {
            if (ModelState.IsValid) {
                CicrequestexecutionRepository.InsertOrUpdate(Cicrequestexecution);
                CicrequestexecutionRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequest = CicrequestRepository.All;
				return View();
			}
        }
        
        //
        // GET: /CicRequestExecution/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleCicRequest = CicrequestRepository.All;
             return View(CicrequestexecutionRepository.Find(id));
        }

        //
        // POST: /CicRequestExecution/Edit/5

        [HttpPost]
        public ActionResult Edit(CicRequestExecution Cicrequestexecution)
        {
            if (ModelState.IsValid) {
                CicrequestexecutionRepository.InsertOrUpdate(Cicrequestexecution);
                CicrequestexecutionRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequest = CicrequestRepository.All;
				return View();
			}
        }

        //
        // GET: /CicRequestExecution/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(CicrequestexecutionRepository.Find(id));
        }

        //
        // POST: /CicRequestExecution/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            CicrequestexecutionRepository.Delete(id);
            CicrequestexecutionRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                CicrequestRepository.Dispose();
                CicrequestexecutionRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

