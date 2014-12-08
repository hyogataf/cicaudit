using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{
  //  [Authorize(Roles = "Administrateur")]
    public class CicRoleController : Controller
    {
        private readonly ICicRoleRepository cicroleRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

        // If you are using Dependency Injection, you can delete the following constructor
        public CicRoleController()
            : this(new CicRoleRepository())
        {
        }

        public CicRoleController(ICicRoleRepository cicroleRepository)
        {
            this.cicroleRepository = cicroleRepository;
        }

        //
        // GET: /CicRole/

        public ViewResult Index()
        {
            return View(cicroleRepository.All);
        }

        //
        // GET: /CicRole/Details/5

        public ViewResult Details(long id)
        {
            return View(cicroleRepository.Find(id));
        }

        //
        // GET: /CicRole/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CicRole/Create

        [HttpPost]
        public ActionResult Create(CicRole cicrole)
        {
            cicrole.DateCreated = DateTime.Now;
            cicrole.UserCreated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            if (ModelState.IsValid)
            {
                cicroleRepository.InsertOrUpdate(cicrole);
                cicroleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CicRole/Edit/5

        public ActionResult Edit(long id)
        {
            return View(cicroleRepository.Find(id));
        }

        //
        // POST: /CicRole/Edit/5

        [HttpPost]
        public ActionResult Edit(CicRole cicrole)
        {
            cicrole.DateUpdated = DateTime.Now;
            cicrole.UserUpdated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            if (ModelState.IsValid)
            {
                cicroleRepository.InsertOrUpdate(cicrole);
                cicroleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CicRole/Delete/5

        public ActionResult Delete(long id)
        {
            return View(cicroleRepository.Find(id));
        }

        //
        // POST: /CicRole/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicroleRepository.Delete(id);
            cicroleRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cicroleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

