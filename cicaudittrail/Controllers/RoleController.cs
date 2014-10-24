using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{   
    public class RoleController : Controller
    {
		private readonly IRoleRepository roleRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public RoleController() : this(new RoleRepository())
        {
        }

        public RoleController(IRoleRepository roleRepository)
        {
			this.roleRepository = roleRepository;
        }

        //
        // GET: /Role/

        public ViewResult Index()
        {
         
            return View(roleRepository.All);
        }

        //
        // GET: /Role/Details/5

        public ViewResult Details(long id)
        {
            return View(roleRepository.Find(id));
        }

        //
        // GET: /Role/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid) {
                roleRepository.InsertOrUpdate(role);
                roleRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Role/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(roleRepository.Find(id));
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid) {
                roleRepository.InsertOrUpdate(role);
                roleRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Role/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(roleRepository.Find(id));
        }

        //
        // POST: /Role/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            roleRepository.Delete(id);
            roleRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                roleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

