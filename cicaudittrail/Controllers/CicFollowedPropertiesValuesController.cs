using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{   
    public class CicFollowedPropertiesValuesController : Controller
    {
		private readonly ICicRequestResultsFollowedRepository cicrequestresultsfollowedRepository;
		private readonly ICicFollowedPropertiesValuesRepository cicfollowedpropertiesvaluesRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CicFollowedPropertiesValuesController() : this(new CicRequestResultsFollowedRepository(), new CicFollowedPropertiesValuesRepository())
        {
        }

        public CicFollowedPropertiesValuesController(ICicRequestResultsFollowedRepository cicrequestresultsfollowedRepository, ICicFollowedPropertiesValuesRepository cicfollowedpropertiesvaluesRepository)
        {
			this.cicrequestresultsfollowedRepository = cicrequestresultsfollowedRepository;
			this.cicfollowedpropertiesvaluesRepository = cicfollowedpropertiesvaluesRepository;
        }

        //
        // GET: /CicFollowedPropertiesValues/

        public ViewResult Index()
        {
            return View(cicfollowedpropertiesvaluesRepository.AllIncluding(cicfollowedpropertiesvalues => cicfollowedpropertiesvalues.CicRequestResultsFollowed));
        }

        //
        // GET: /CicFollowedPropertiesValues/Details/5

        public ViewResult Details(long id)
        {
            return View(cicfollowedpropertiesvaluesRepository.Find(id));
        }

        //
        // GET: /CicFollowedPropertiesValues/Create

        public ActionResult Create()
        {
			ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
            return View();
        } 

        //
        // POST: /CicFollowedPropertiesValues/Create

        [HttpPost]
        public ActionResult Create(CicFollowedPropertiesValues cicfollowedpropertiesvalues)
        {
            if (ModelState.IsValid) {
                cicfollowedpropertiesvaluesRepository.InsertOrUpdate(cicfollowedpropertiesvalues);
                cicfollowedpropertiesvaluesRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
				return View();
			}
        }
        
        //
        // GET: /CicFollowedPropertiesValues/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
             return View(cicfollowedpropertiesvaluesRepository.Find(id));
        }

        //
        // POST: /CicFollowedPropertiesValues/Edit/5

        [HttpPost]
        public ActionResult Edit(CicFollowedPropertiesValues cicfollowedpropertiesvalues)
        {
            if (ModelState.IsValid) {
                cicfollowedpropertiesvaluesRepository.InsertOrUpdate(cicfollowedpropertiesvalues);
                cicfollowedpropertiesvaluesRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleCicRequestResultsFollowed = cicrequestresultsfollowedRepository.All;
				return View();
			}
        }

        //
        // GET: /CicFollowedPropertiesValues/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(cicfollowedpropertiesvaluesRepository.Find(id));
        }

        //
        // POST: /CicFollowedPropertiesValues/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicfollowedpropertiesvaluesRepository.Delete(id);
            cicfollowedpropertiesvaluesRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                cicrequestresultsfollowedRepository.Dispose();
                cicfollowedpropertiesvaluesRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

