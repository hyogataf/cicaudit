using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;

namespace cicaudittrail.Controllers
{
   // [Authorize(Roles = "Administrateur")]
    public class CicUserRoleController : Controller
    {
        private readonly ICicRoleRepository cicroleRepository;
        private readonly ICicUserRoleRepository cicuserroleRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

        // If you are using Dependency Injection, you can delete the following constructor
        public CicUserRoleController()
            : this(new CicRoleRepository(), new CicUserRoleRepository())
        {
        }

        public CicUserRoleController(ICicRoleRepository cicroleRepository, ICicUserRoleRepository cicuserroleRepository)
        {
            this.cicroleRepository = cicroleRepository;
            this.cicuserroleRepository = cicuserroleRepository;
        }

        //
        // GET: /CicUserRole/

        public ViewResult Index()
        {
            return View(cicuserroleRepository.AllIncluding(cicuserrole => cicuserrole.CicRole));
        }

        //
        // GET: /CicUserRole/Details/5

        public ViewResult Details(long id)
        {
            return View(cicuserroleRepository.Find(id));
        }

        //
        // GET: /CicUserRole/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCicRole = cicroleRepository.All;
            return View();
        }

        //
        // POST: /CicUserRole/Create

        [HttpPost]
        public ActionResult Create(CicUserRole cicuserrole)
        {
            cicuserrole.DateCreated = DateTime.Now;
            cicuserrole.UserCreated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            if (ModelState.IsValid)
            {
                cicuserroleRepository.InsertOrUpdate(cicuserrole);
                cicuserroleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRole = cicroleRepository.All;
                return View();
            }
        }

        //
        // GET: /CicUserRole/Edit/5

        public ActionResult Edit(long id)
        {
            ViewBag.PossibleCicRole = cicroleRepository.All;
            return View(cicuserroleRepository.Find(id));
        }

        //
        // POST: /CicUserRole/Edit/5

        [HttpPost]
        public ActionResult Edit(CicUserRole cicuserrole)
        {
            cicuserrole.DateUpdated = DateTime.Now;
            cicuserrole.UserUpdated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            if (ModelState.IsValid)
            {
                cicuserroleRepository.InsertOrUpdate(cicuserrole);
                cicuserroleRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRole = cicroleRepository.All;
                return View();
            }
        }

        //
        // GET: /CicUserRole/Delete/5

        public ActionResult Delete(long id)
        {
            return View(cicuserroleRepository.Find(id));
        }

        //
        // POST: /CicUserRole/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            cicuserroleRepository.Delete(id);
            cicuserroleRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cicroleRepository.Dispose();
                cicuserroleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

