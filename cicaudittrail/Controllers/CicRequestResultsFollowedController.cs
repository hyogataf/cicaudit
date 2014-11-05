using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using cicaudittrail.Src;
using System.Threading.Tasks;

namespace cicaudittrail.Controllers
{
    public class CicRequestResultsFollowedController : Controller
    {
        private readonly ICicRequestRepository CicrequestRepository;
        private readonly ICicRequestResultsFollowedRepository CicrequestresultsfollowedRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public CicRequestResultsFollowedController()
            : this(new CicRequestRepository(), new CicRequestResultsFollowedRepository())
        {
        }

        public CicRequestResultsFollowedController(ICicRequestRepository CicrequestRepository, ICicRequestResultsFollowedRepository CicrequestresultsfollowedRepository)
        {
            this.CicrequestRepository = CicrequestRepository;
            this.CicrequestresultsfollowedRepository = CicrequestresultsfollowedRepository;
        }

        //
        // GET: /CicRequestResultsFollowed/

        public ViewResult Index()
        {
            return View(CicrequestresultsfollowedRepository.AllIncluding(Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
        }

        //
        // GET: /CicRequestResultsFollowed/

        public ViewResult SendMailGestionnaire()
        {
            return View(CicrequestresultsfollowedRepository.AllIncluding(Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
        }

        //
        // GET: /CicRequestResultsFollowed/IndexByRequest/5

        public ViewResult IndexByRequest(long id)
        {
            CicRequest CicRequest = CicrequestRepository.Find(id);
            //List<CicRequestResultsFollowed> CicrequestresultsfollowedList = new List<CicRequestResultsFollowed>();
            if (CicRequest == null)
            {
                TempData["error"] = "Aucune requête avec l'id " + id + " n'a été trouvée.";
                return View("/CicRequest/Index");
            }
            ViewData["CicRequest.Code"] = CicRequest.Code;
            ViewData["CicRequest.Libelle"] = CicRequest.Libelle;
            ViewData["CicRequest.Id"] = CicRequest.CicRequestId;

            return View(CicrequestresultsfollowedRepository.AllByRequest(id));
        }


        //
        // GET: /CicRequestResultsFollowed/Details/5

        public ActionResult Details(long id)
        {
            var model = new List<object[]>();
            var listResults = new List<CicRequestResultsFollowed>();
            var CicrequestresultsfollowedInstance = CicrequestresultsfollowedRepository.Find(id);
            if (CicrequestresultsfollowedInstance == null)
            {
                // Debug.WriteLine("CicrequestresultsfollowedInstance null");
                TempData["error"] = "Veuillez vérifier vos suivis. Aucun enregistrement avec l'id " + id + " n'a été trouvée.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(CicrequestresultsfollowedInstance.RowContent))
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance RowContent empty");
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête avec l'id " + id + " n'a été trouvée.";
                return View(CicrequestresultsfollowedInstance);
            }
            listResults.Add(CicrequestresultsfollowedInstance);
            model = Read(listResults).ToList();
            ViewData["Comments"] = CicrequestresultsfollowedInstance.Comments;
            ViewData["Libelle"] = CicrequestresultsfollowedInstance.CicRequest.Libelle;
            ViewData["DateCreated"] = CicrequestresultsfollowedInstance.DateCreated;
            ViewData["UserCreated"] = CicrequestresultsfollowedInstance.UserCreated;

            return View(model);
        }


        //
        // GET: /CicRequestResultsFollowed/Details/5

        public ActionResult AjaxDetails(long id)
        {
            var model = new List<object[]>();
            var listResults = new List<CicRequestResultsFollowed>();
            var CicrequestresultsfollowedInstance = CicrequestresultsfollowedRepository.Find(id);
            if (CicrequestresultsfollowedInstance == null)
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance null");
                TempData["error"] = "Veuillez vérifier vos suivis. Aucun enregistrement avec l'id " + id + " n'a été trouvée.";
                //return RedirectToAction("Index");
                return PartialView("_DetailsFollowed", model);
            }

            if (string.IsNullOrEmpty(CicrequestresultsfollowedInstance.RowContent))
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance RowContent empty");
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête avec l'id " + id + " n'a été trouvée.";
                //return View(CicrequestresultsfollowedInstance);
                return PartialView("_DetailsFollowed", model);
            }
            listResults.Add(CicrequestresultsfollowedInstance);
            model = Read(listResults).ToList();

            return PartialView("_DetailsFollowed", model);
            //return View(model);
        }

        //Methode de lecture des données du champ RowContent.
        //Ce champ contient les resultats des requetes parametrées. Ils sont en JSON, de la forme:
        /*
         * {
  "AGE": "01316",
  "AGENCE": "CBAO THIAROYE                 ",
  "CLI": "0012680        ",
  "CHA": "251101    ",
  "NumCompte": "01268007011",
  "INTI": "MATAR DIAGNE/ECID             ",
  "DateComptable": "2014-10-10T00:00:00",
  "SEN": "D",
  "Montant": 10000000.0,
  "Libelle": "RETRAIT CH 9407875 MATAR DIAG ",
  "SoldeAJ": -432348.0,
  "Utilisateur": "BNSY      ",
  "UtiAyantForce": "BNSY      ",
  "GES": "637",
  "GESTIONNAIRE": "SAMBA YAYE CISSE              ",
  "NMBRE": 1.0,
  "IDENTIFICATION": "SNDKR2004A16954     ",
  "SECTEUR": "Particuliers                  ",
  "NATIONALITÉ": "Sénégalaise                   ",
  "PROFESSION": null,
  "MvtsAuCredit": 53774202.0,
  "MvtsAuDebit": 54270680.0
}
         * */
        //Cette methode recupere plusieurs lignes de cette forme et les presente sous forme de tableau dans la vue Index, 
        //avec les clés en entete, et les valeurs dans le corps.
        // NB: on aurait pu diminuer le couplage, en envoyant le JSON, mais on a besoin de l'id du CicRequestResults, so...
        private static IEnumerable<object[]> Read(List<CicRequestResultsFollowed> resultsList)
        {
            //Iteration sur 2 valeurs: la 1ère permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
            for (var i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    var values = new List<object>();
                    var line = resultsList[0];
                    values.Add(line.CicRequestResultsFollowedId);

                    JToken outer = JToken.Parse(line.RowContent);
                    JObject inner = outer.Value<JObject>();
                    List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                    foreach (string k in keys)
                    {
                        values.Add(k);
                    }
                    yield return values.ToArray();
                }
                else
                {
                    foreach (var line in resultsList)
                    {
                        var values = new List<object>();
                        values.Add(line.CicRequestResultsFollowedId);

                        JToken outer = JToken.Parse(line.RowContent);
                        JObject inner = outer.Value<JObject>();
                        List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                        foreach (string k in keys)
                        {
                            values.Add(inner.GetValue(k));
                        }
                        yield return values.ToArray();
                    }
                }
            }
        }

        //
        // GET: /CicRequestResultsFollowed/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View();
        }


        //
        // GET: /CicRequestResultsFollowed/Create

        public ActionResult Search()
        {
            ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View();
        }


        //
        // Get: /CicRequestResultsFollowed/Executesearch

        // [HttpPost]
        public ActionResult Executesearch(string CicRequestId, string DateCreated, string UserCreated, string content)
        {
            //  Debug.WriteLine("requests = " + DateCreated);

            //var results = CicrequestresultsfollowedRepository.ExecuteSearch(Request.Form["CicRequestId"], Request.Form["DateCreated"], Request.Form["UserCreated"], Request.Form["content"], Request.Form["content"]);

            var results = CicrequestresultsfollowedRepository.ExecuteSearch(CicRequestId, DateCreated, UserCreated, content, content);

            // Debug.WriteLine("results Count = " + results.Count());
            if (results.Count() > 0)
                //  Debug.WriteLine("results First = " + results.First().CicRequestResultsFollowedId);

                ViewBag.resultList = results;
            return PartialView("_ResultSearch", results);

        }

        //
        // POST: /CicRequestResultsFollowed/Create

        [HttpPost]
        public ActionResult Create(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            if (ModelState.IsValid)
            {
                CicrequestresultsfollowedRepository.InsertOrUpdate(Cicrequestresultsfollowed);
                CicrequestresultsfollowedRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRequest = CicrequestRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequestResultsFollowed/Edit/5

        public ActionResult Edit(long id)
        {
            ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View(CicrequestresultsfollowedRepository.Find(id));
        }

        //
        // POST: /CicRequestResultsFollowed/Edit/5

        [HttpPost]
        public ActionResult Edit(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            if (ModelState.IsValid)
            {
                CicrequestresultsfollowedRepository.InsertOrUpdate(Cicrequestresultsfollowed);
                CicrequestresultsfollowedRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRequest = CicrequestRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequestResultsFollowed/Delete/5

        public ActionResult Delete(long id)
        {
            return View(CicrequestresultsfollowedRepository.Find(id));
        }

        //
        // POST: /CicRequestResultsFollowed/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            CicrequestresultsfollowedRepository.Delete(id);
            CicrequestresultsfollowedRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CicrequestRepository.Dispose();
                CicrequestresultsfollowedRepository.Dispose();
            }
            base.Dispose(disposing);
        }


        /*
         * Methode d'enregistrement des CicMessageMail.
         * Elle reçoit des id de CicRequestResultsFollowed. Elle recupere le messageTemplate associé, afin d'avoir le contenu du message mail, fait un binding afin de remplacer les variables contenues dans le messageTemplate, puis crée un enregistrement de CicMessageMail.
        */
        // POST: /CicRequestResultsFollowed/PopulateBody

        [HttpPost]
        public ActionResult PopulateBody(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();
            CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
            var nbre = 0;
            HashSet<CicMessageMail> ListCicMessageMail = new HashSet<CicMessageMail>();
            foreach (var k in Request.Form.AllKeys)
            {
                if (k.ToString().StartsWith("coche."))
                {
                    var resultId = k.Split('.')[1];
                    //si case cochée <==> si on veut envoyer msg pour la ligne en cours
                    if (Request.Form.GetValues(k.ToString())[0] == "true")
                    {
                        CicRequestResultsFollowed CicRequestFollowedInstance = CicrequestresultsfollowedRepository.Find(Convert.ToInt64(resultId));
                        if (CicRequestFollowedInstance != null && CicRequestFollowedInstance.CicRequest.CicMessageTemplateId != null)
                        {
                            Dictionary<string, string> map = new Dictionary<string, string>();
                            map.Add("#{NomGestionnaire}", "admin");
                            ToolsClass toolsClass = new ToolsClass();
                            var objet = "Suivi " + CicRequestFollowedInstance.CicRequestResultsFollowedId + ": " + CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ObjetMail;
                            var bodyMessage = CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ContenuMail;
                            var CicMessageMail = new CicMessageMail();
                            CicMessageMail.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                            CicMessageMail.DateMessageSent = DateTime.Now;
                            CicMessageMail.MessageSent = toolsClass.generateBodyMessage(map, bodyMessage);
                            CicMessageMail.ObjetMessage = objet;
                            CicMessageMail.UserMessageSent = "admin"; //TODO recuperer le user connecté
                            CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                            CicMessageMailRepository.Save();
                            ListCicMessageMail.Add(CicMessageMail);


                            //Enregistrement de CicRequestExecution
                            var CicRequestExecutionInstance = new CicRequestExecution();
                            CicRequestExecutionInstance.CicRequestId = CicRequestFollowedInstance.CicRequestId;
                            CicRequestExecutionInstance.UserAction = "admin"; //TODO mettre le user connecté
                            CicRequestExecutionInstance.DateAction = DateTime.Now;
                            CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.MS.ToString();
                            CicRequestExecutionInstance.DateCreated = DateTime.Now;
                            CicRequestExecutionInstance.CicMessageMailId = CicMessageMail.CicMessageMailId;
                            CicRequestExecutionInstance.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                            CicrequestExecutionRepository.Save();
                            nbre++;
                        }
                    }
                }
            }

            //envoi du mail
            Task.Factory.StartNew(() =>
            {
                MailingClass MailingClass = new MailingClass();
                foreach (CicMessageMail mail in ListCicMessageMail)
                {
                    MailingClass.SendEmail("c.hyoga@hotmail.com", mail.ObjetMessage, mail.MessageSent);
                }
            });
            TempData["message"] = nbre + " message(s) a (ont) été envoyé avec succés";
            return RedirectToAction("Index");
        }
    }
}

