using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;
using System.Diagnostics;

namespace cicaudittrail.Controllers
{
    public class CicRequestController : Controller
    {
        private readonly ICicRequestRepository CicrequestRepository;
        private readonly ICicRequestExecutionRepository CicrequestExecutionRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

        // If you are using Dependency Injection, you can delete the following constructor
        public CicRequestController()
            : this(new CicRequestRepository(), new CicRequestExecutionRepository())
        {
        }

        public CicRequestController(ICicRequestRepository CicrequestRepository, ICicRequestExecutionRepository CicrequestExecutionRepository)
        {
            this.CicrequestRepository = CicrequestRepository;
            this.CicrequestExecutionRepository = CicrequestExecutionRepository;
        }

        //
        // GET: /CicRequest/

        public ViewResult Index()
        {
            System.Web.Security.FormsIdentity identity = (System.Web.Security.FormsIdentity)System.Web.HttpContext.Current.User.Identity;

            /*   Debug.WriteLine("identity = " + identity.Name);
               Debug.WriteLine("identity = " + identity.IsAuthenticated);
               var UserFromDb = System.Web.Security.Membership.GetUser();
               Debug.WriteLine("UserFromDb = " + UserFromDb);
               Debug.WriteLine("UserFromDb = " + UserFromDb.UserName);
               Debug.WriteLine("UserFromDb = " + UserFromDb.Email);
               Debug.WriteLine("Session = " + Session["CurrentUser"]);
               Debug.WriteLine("Session type = " + Session["CurrentUser"].GetType());*/

            return View(CicrequestRepository.All);
        }


        //
        // GET: /CicRequest/Details/5

        public ViewResult Details(long id)
        {
            return View(CicrequestRepository.Find(id));
        }

        //
        // GET: /CicRequest/Create

        public ActionResult Create()
        {
            CicMessageTemplateRepository CicMessageTemplateRepository = new CicMessageTemplateRepository();
            ViewBag.PossibleCicMessageTemplate = CicMessageTemplateRepository.All;
            return View();
        }

        //
        // POST: /CicRequest/Create

        [HttpPost]
        public ActionResult Create(CicRequest Cicrequest)
        {
            if (ModelState.IsValid)
            {
                CicrequestRepository.InsertOrUpdate(Cicrequest);
                CicrequestRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                CicMessageTemplateRepository CicMessageTemplateRepository = new CicMessageTemplateRepository();
                ViewBag.PossibleCicMessageTemplate = CicMessageTemplateRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequest/Edit/5
        [Authorize(Roles = "Requeteur")]
        public ActionResult Edit(long id)
        {
            CicMessageTemplateRepository CicMessageTemplateRepository = new CicMessageTemplateRepository();
            ViewBag.PossibleCicMessageTemplate = CicMessageTemplateRepository.All;
            return View(CicrequestRepository.Find(id));
        }

        //
        // POST: /CicRequest/Edit/5

        [Authorize(Roles = "Requeteur")]
        [HttpPost]
        public ActionResult Edit(CicRequest Cicrequest)
        {
            if (ModelState.IsValid)
            {
                CicrequestRepository.InsertOrUpdate(Cicrequest);
                CicrequestRepository.Save();

                //Enregistrement de CicRequestExecution
                var CicRequestExecutionInstance = new CicRequestExecution();
                CicRequestExecutionInstance.CicRequestId = Cicrequest.CicRequestId;

                CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                CicRequestExecutionInstance.DateAction = DateTime.Now;
                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.U.ToString();
                CicRequestExecutionInstance.DateCreated = DateTime.Now;
                CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                CicrequestExecutionRepository.Save();

                return RedirectToAction("Index");
            }
            else
            {
                CicMessageTemplateRepository CicMessageTemplateRepository = new CicMessageTemplateRepository();
                ViewBag.PossibleCicMessageTemplate = CicMessageTemplateRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequest/Delete/5
        [Authorize(Roles = "Requeteur")]
        public ActionResult Delete(long id)
        {
            return View(CicrequestRepository.Find(id));
        }

        //
        // POST: /CicRequest/Delete/5
        [Authorize(Roles = "Requeteur")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            CicrequestRepository.Delete(id);
            CicrequestRepository.Save();

            //Enregistrement de CicRequestExecution
            var CicRequestExecutionInstance = new CicRequestExecution();
            CicRequestExecutionInstance.CicRequestId = id;
            CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString(); //TODO mettre le user connecté
            CicRequestExecutionInstance.DateAction = DateTime.Now;
            CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.D.ToString();
            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
            CicrequestExecutionRepository.Save();

            return RedirectToAction("Index");
        }

        // GET: /CicRequest/Execute/5
        public ActionResult Execute(long id)
        {

            /*  try
              {*/
            var requestInstance = CicrequestRepository.Find(id);
            if (requestInstance == null)
            {
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête avec l'id " + id + " n'a été trouvée.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(requestInstance.Request))
            {
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête SQL detectée";
                return RedirectToAction("Index");
            }
            return View();
        }

        private static IEnumerable<Dictionary<string, object>> ReadToDictionary(DbDataReader reader)
        {
            while (reader.Read())
            {
                var values = new List<Dictionary<string, object>>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var dict = new Dictionary<string, object>();
                    dict.Add(reader.GetName(i), reader.GetValue(i));
                    yield return dict;
                }
                //yield return values.ToArray();
            }
        }


        private static IEnumerable<object[]> Read(DbDataReader reader)
        {
            var count = 0;
            while (reader.Read())
            {
                var values = new List<object>();
                if (count == 0)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader.GetName(i));
                    }
                }
                else
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader.GetValue(i));
                    }
                }
                count++;
                yield return values.ToArray();
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CicrequestRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

