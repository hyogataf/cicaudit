using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;
using System.Diagnostics;
using System.Data.Common;
using Newtonsoft.Json;

namespace cicaudittrail.Controllers
{
    public class CicRequestController : Controller
    {
        private readonly ICicRequestRepository CicrequestRepository;
        private readonly ICicRequestExecutionRepository CicrequestExecutionRepository;

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
                return View();
            }
        }

        //
        // GET: /CicRequest/Edit/5

        public ActionResult Edit(long id)
        {
            return View(CicrequestRepository.Find(id));
        }

        //
        // POST: /CicRequest/Edit/5

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
                CicRequestExecutionInstance.CicRequestUserUpdated = "admin"; // TODO mettre le user connecté
                CicRequestExecutionInstance.CicRequestDateUpdated = DateTime.Now;
                CicRequestExecutionInstance.DateCreated = DateTime.Now;
                CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                CicrequestExecutionRepository.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CicRequest/Delete/5

        public ActionResult Delete(long id)
        {
            return View(CicrequestRepository.Find(id));
        }

        //
        // POST: /CicRequest/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            CicrequestRepository.Delete(id);
            CicrequestRepository.Save();

            //Enregistrement de CicRequestExecution
            var CicRequestExecutionInstance = new CicRequestExecution();
            CicRequestExecutionInstance.CicRequestId = id;
            CicRequestExecutionInstance.CicRequestUserDeleted = "admin"; // TODO mettre le user connecté
            CicRequestExecutionInstance.CicRequestDateDeleted = DateTime.Now;
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

