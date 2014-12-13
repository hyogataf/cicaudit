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
using System.Reflection;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Collections;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Novacode;

namespace cicaudittrail.Controllers
{
    public class CicRequestResultsFollowedController : Controller
    {
        private readonly ICicRequestRepository CicrequestRepository;
        private readonly ICicRequestResultsFollowedRepository CicrequestresultsfollowedRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

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

        public ViewResult IndexForMailResponses()
        {
            string[] status = new string[] { "MR", "S" };
            return View(CicrequestresultsfollowedRepository.AllAnsweredIncluding(status, Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
        }


        //
        // GET: /CicRequestResultsFollowed/

        public ViewResult IndexForCentif()
        {
            string[] status = new string[] { "S", "C" };
            return View(CicrequestresultsfollowedRepository.AllAnsweredIncluding(status, Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
        }

        //
        // GET: /CicRequestResultsFollowed/

        public ViewResult SendMailGestionnaire()
        {
            string[] status = new string[] { "E", "ME", "MR" };
            return View(CicrequestresultsfollowedRepository.AllAnsweredIncluding(status, Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
            // return View(CicrequestresultsfollowedRepository.AllIncluding(Cicrequestresultsfollowed => Cicrequestresultsfollowed.CicRequest));
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
            ViewData["Id"] = CicrequestresultsfollowedInstance.CicRequestResultsFollowedId;
            ViewData["Statut"] = CicrequestresultsfollowedInstance.Statut;

            return View(model);
        }



        //
        // GET: /CicRequestResultsFollowed/Details/5

        public ActionResult DetailsPrintMessages(long id)
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

            return View(CicrequestresultsfollowedInstance);
            // return View(model);
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
            //Debug.WriteLine("Request.Form = " + Request.Form);
            CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();
            CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
            var nbre = 0;
            //Liste des mails à envoyer
            HashSet<CicMessageMail> ListCicMessageMail = new HashSet<CicMessageMail>();
            //Liste des erreurs d'envois
            HashSet<CicRequestResultsFollowed> ListErrorMail = new HashSet<CicRequestResultsFollowed>();

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
                            // recuperer dans la requete CicRequest le nom et le mail du gestionnaire
                            var NomGestionnaire = ""; var email = "";
                            CicFollowedPropertiesValuesRepository CicFollowedPropertiesValuesRepository = new CicFollowedPropertiesValuesRepository();
                            var propGestInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(CicRequestFollowedInstance.CicRequestResultsFollowedId, "Gestionnaire");
                            if (propGestInstance != null) NomGestionnaire = propGestInstance.Value;

                            var propEmailInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(CicRequestFollowedInstance.CicRequestResultsFollowedId, "MailGestionnaire"); // TODO verifier validité mail regex 
                            if (propEmailInstance != null) email = propEmailInstance.Value;
                            //  else email = "ahad.fall@cbao.sn";// TODO delete line

                            //on remplit les variables du message template
                            map.Add("#{NomGestionnaire}", NomGestionnaire);
                            ToolsClass toolsClass = new ToolsClass();
                            var objet = "Suivi " + CicRequestFollowedInstance.CicRequestResultsFollowedId + ": " + CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ObjetMail;
                            var bodyMessage = CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ContenuMail;

                            //Enregistrement du mail dans la table CicMessageMail
                            // on envoie le mail que si l'adresse mail du gestionnaire est renseignée
                            if (email != null && email != "")
                            {
                                var CicMessageMail = new CicMessageMail();
                                CicMessageMail.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                                CicMessageMail.DateMessage = DateTime.Now;
                                CicMessageMail.MessageContent = toolsClass.generateBodyMessage(map, bodyMessage);
                                CicMessageMail.ObjetMessage = objet;
                                CicMessageMail.Email = email;
                                CicMessageMail.UserMessage = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                                CicMessageMail.Sens = Sens.O.ToString();
                                CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                                CicMessageMailRepository.Save();

                                ListCicMessageMail.Add(CicMessageMail);



                                //Generation fichier excel (csv) 
                                var RowContentcsv = generateCSV(CicRequestFollowedInstance.RowContent);
                                //  Debug.WriteLine("RowContentcsv = " + RowContentcsv);
                                // transform en byte
                                byte[] RowContentbytes = Encoding.Default.GetBytes(RowContentcsv);

                                //Enregistrement de la piece jointe dans la table CicMessageMailDocuments
                                CicMessageMailDocuments CicMessageMailDocuments = new CicMessageMailDocuments();
                                CicMessageMailDocuments.CicMessageMailId = CicMessageMail.CicMessageMailId;
                                CicMessageMailDocuments.DocumentName = "Operation_douteuse.csv";
                                CicMessageMailDocuments.DocumentType = "application/csv";
                                CicMessageMailDocuments.Document = RowContentbytes;
                                CicMessageMailDocuments.DateCreated = DateTime.Now;
                                CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();
                                CicMessageMailDocumentsRepository.InsertOrUpdate(CicMessageMailDocuments);
                                CicMessageMailDocumentsRepository.Save();

                                //Enregistrement de CicRequestExecution (audit log)
                                var CicRequestExecutionInstance = new CicRequestExecution();
                                CicRequestExecutionInstance.CicRequestId = CicRequestFollowedInstance.CicRequestId;
                                CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                                CicRequestExecutionInstance.DateAction = DateTime.Now;
                                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.MS.ToString();
                                CicRequestExecutionInstance.DateCreated = DateTime.Now;
                                CicRequestExecutionInstance.CicMessageMailId = CicMessageMail.CicMessageMailId;
                                CicRequestExecutionInstance.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                                CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                                CicrequestExecutionRepository.Save();
                                nbre++;

                                //Update de CicRequestResultsFollowed
                                // on ne flag le suivi que si le adresse mail du gestionnaire est renseignée
                                // sinon on l'enregistre dans la liste des erreurs
                                /* if (email != null && email != "")
                                 {*/
                                CicRequestFollowedInstance.Statut = Statut.ME.ToString();
                                CicrequestresultsfollowedRepository.InsertOrUpdate(CicRequestFollowedInstance);
                                CicrequestresultsfollowedRepository.Save();
                            }
                            else ListErrorMail.Add(CicRequestFollowedInstance);
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
                    CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();

                    ArrayList mailArray = new ArrayList();
                    //  JArray mailArray = new JArray();
                    mailArray.Add(JsonConvert.SerializeObject(mail.ConvertToMessageEntity()));
                    var MessageMailDocumentsList = CicMessageMailDocumentsRepository.FindAllByCicMessageMail(mail.CicMessageMailId);
                    foreach (CicMessageMailDocuments msgdoc in MessageMailDocumentsList)
                    {
                        //mailArray.Add(JsonConvert.SerializeObject(msgdoc.ConvertToMessageEntityPJ()));
                        mailArray.Add(JsonConvert.SerializeObject(msgdoc.ConvertToMessageEntityPJ()));
                    }

                    var rstl = MailingClass.SendEmail(mailArray);
                    Debug.WriteLine("MailingClass rstl = " + rstl);
                    if (rstl == true)
                    {
                        mail.IsSent = IsSent.OK.ToString();
                        CicMessageMailRepository.InsertOrUpdate(mail);
                        CicMessageMailRepository.Save();
                    }
                    else
                    {
                        mail.IsSent = IsSent.KO.ToString();
                        CicMessageMailRepository.InsertOrUpdate(mail);
                        CicMessageMailRepository.Save();
                    }
                }
            });
            if (ListErrorMail.Count > 0)
            {
                Debug.WriteLine("ListErrorMail = " + ListErrorMail.Count);

                // TODO return vue erreur liste erreurs
                if (ListErrorMail.Count == 1) TempData["error"] = " Aucune adresse email pour ce suivi n'a pas pu être detectée. Veuillez la saisir";
                else TempData["error"] = ListErrorMail.Count + " adresses emails n'ont pas pu être detectées. Veuillez les saisir";

                TempData["ListErrors"] = ListErrorMail;
                //  ViewBag.ListErrors = ListErrorMail;
                return RedirectToAction("SendMailGestionnaireManuel", new { ListErrorMail });
            }
            else
            {
                TempData["message"] = nbre + " message(s) a (ont) été envoyé avec succés";
                return RedirectToAction("Index");
            }
        }


        public ActionResult PopulateBodyManualMailing(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            Debug.WriteLine("Request.Form = " + Request.Form);
            foreach (var k in Request.Form.AllKeys)
            {
                Debug.WriteLine(" k = " + k);
                if (k.ToString().StartsWith("mail."))
                {
                    var resultId = k.Split('.')[1];
                    Debug.WriteLine(" resultId = " + resultId);
                    var te = Request.Form.GetValues(k.ToString())[0];
                    Debug.WriteLine(" te = " + te);
                }


            }

            CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();
            CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
            var nbre = 0;
            //Liste des mails à envoyer
            HashSet<CicMessageMail> ListCicMessageMail = new HashSet<CicMessageMail>();
            //Liste des erreurs d'envois
            HashSet<CicRequestResultsFollowed> ListErrorMail = new HashSet<CicRequestResultsFollowed>();

            foreach (var k in Request.Form.AllKeys)
            {
                if (k.ToString().StartsWith("mail."))
                {
                    var resultId = k.Split('.')[1];
                    //si case cochée <==> si on veut envoyer msg pour la ligne en cours

                    if (Request.Form.GetValues(k.ToString())[0].Contains("@"))
                    {
                        CicRequestResultsFollowed CicRequestFollowedInstance = CicrequestresultsfollowedRepository.Find(Convert.ToInt64(resultId));

                        if (CicRequestFollowedInstance != null && CicRequestFollowedInstance.CicRequest.CicMessageTemplateId != null)
                        {
                            Dictionary<string, string> map = new Dictionary<string, string>();
                            // recuperer dans la requete CicRequest le nom et le mail du gestionnaire
                            var NomGestionnaire = ""; var email = "";
                            CicFollowedPropertiesValuesRepository CicFollowedPropertiesValuesRepository = new CicFollowedPropertiesValuesRepository();
                            var propGestInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(CicRequestFollowedInstance.CicRequestResultsFollowedId, "Gestionnaire");
                            if (propGestInstance != null) NomGestionnaire = propGestInstance.Value;

                            email = Request.Form.GetValues(k.ToString())[0];
                            //  else email = "ahad.fall@cbao.sn";// TODO delete line

                            //on remplit les variables du message template
                            map.Add("#{NomGestionnaire}", NomGestionnaire);
                            ToolsClass toolsClass = new ToolsClass();
                            var objet = "Suivi " + CicRequestFollowedInstance.CicRequestResultsFollowedId + ": " + CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ObjetMail;
                            var bodyMessage = CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ContenuMail;

                            if (email != null && email != "")
                            {
                                //Enregistrement du mail dans la table CicMessageMail
                                var CicMessageMail = new CicMessageMail();
                                CicMessageMail.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                                CicMessageMail.DateMessage = DateTime.Now;
                                CicMessageMail.MessageContent = toolsClass.generateBodyMessage(map, bodyMessage);
                                CicMessageMail.ObjetMessage = objet;
                                CicMessageMail.Email = email;
                                CicMessageMail.UserMessage = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                                CicMessageMail.Sens = Sens.O.ToString();
                                CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                                CicMessageMailRepository.Save();
                                // on envoie le mail que si l'adresse mail du gestionnaire est renseignée

                                ListCicMessageMail.Add(CicMessageMail);


                                //Generation fichier excel (csv) 
                                var RowContentcsv = generateCSV(CicRequestFollowedInstance.RowContent);
                                //  Debug.WriteLine("RowContentcsv = " + RowContentcsv);
                                // transform en byte
                                byte[] RowContentbytes = Encoding.Default.GetBytes(RowContentcsv);

                                //Enregistrement de la piece jointe dans la table CicMessageMailDocuments
                                CicMessageMailDocuments CicMessageMailDocuments = new CicMessageMailDocuments();
                                CicMessageMailDocuments.CicMessageMailId = CicMessageMail.CicMessageMailId;
                                CicMessageMailDocuments.DocumentName = "Operation_douteuse.csv";
                                CicMessageMailDocuments.DocumentType = "application/csv";
                                CicMessageMailDocuments.Document = RowContentbytes;
                                CicMessageMailDocuments.DateCreated = DateTime.Now;
                                CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();
                                CicMessageMailDocumentsRepository.InsertOrUpdate(CicMessageMailDocuments);
                                CicMessageMailDocumentsRepository.Save();

                                //Update de CicRequestResultsFollowed
                                // on ne flag le suivi que si le adresse mail du gestionnaire est renseignée
                                // sinon on l'enregistre dans la liste des erreurs
                                /* if (email != null && email != "")
                                 {*/
                                CicRequestFollowedInstance.Statut = Statut.ME.ToString();
                                CicrequestresultsfollowedRepository.InsertOrUpdate(CicRequestFollowedInstance);
                                CicrequestresultsfollowedRepository.Save();

                                //Enregistrement de CicRequestExecution (audit log)
                                var CicRequestExecutionInstance = new CicRequestExecution();
                                CicRequestExecutionInstance.CicRequestId = CicRequestFollowedInstance.CicRequestId;
                                CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                                CicRequestExecutionInstance.DateAction = DateTime.Now;
                                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.MS.ToString();
                                CicRequestExecutionInstance.DateCreated = DateTime.Now;
                                CicRequestExecutionInstance.CicMessageMailId = CicMessageMail.CicMessageMailId;
                                CicRequestExecutionInstance.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                                CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                                CicrequestExecutionRepository.Save();
                                nbre++;
                            }
                            else ListErrorMail.Add(CicRequestFollowedInstance);
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
                    CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();

                    ArrayList mailArray = new ArrayList();
                    //  JArray mailArray = new JArray();
                    mailArray.Add(JsonConvert.SerializeObject(mail.ConvertToMessageEntity()));
                    var MessageMailDocumentsList = CicMessageMailDocumentsRepository.FindAllByCicMessageMail(mail.CicMessageMailId);
                    foreach (CicMessageMailDocuments msgdoc in MessageMailDocumentsList)
                    {
                        //mailArray.Add(JsonConvert.SerializeObject(msgdoc.ConvertToMessageEntityPJ()));
                        mailArray.Add(JsonConvert.SerializeObject(msgdoc.ConvertToMessageEntityPJ()));
                    }

                    var rstl = MailingClass.SendEmail(mailArray);
                    Debug.WriteLine("MailingClass rstl = " + rstl);
                    if (rstl == true)
                    {
                        mail.IsSent = IsSent.OK.ToString();
                        CicMessageMailRepository.InsertOrUpdate(mail);
                        CicMessageMailRepository.Save();
                    }
                    else
                    {
                        mail.IsSent = IsSent.KO.ToString();
                        CicMessageMailRepository.InsertOrUpdate(mail);
                        CicMessageMailRepository.Save();
                    }
                }
            });
            if (ListErrorMail.Count > 0)
            {
                Debug.WriteLine("ListErrorMail = " + ListErrorMail.Count);

                // TODO return vue erreur liste erreurs
                TempData["error"] = ListErrorMail.Count + " adresses emails n'ont pas pu être detectées. Veuillez les saisir";
                ViewBag.ListErrors = ListErrorMail;
                return RedirectToAction("SendMailGestionnaireManuel", ListErrorMail);
            }
            else
            {
                TempData["message"] = nbre + " message(s) a (ont) été envoyé avec succés";
                return RedirectToAction("Index");
            }
        }


        private string generateCSV(string RowContent)
        {
            //On eneleve les caracteres non acceptés par DeserializeXmlNode
            ToolsClass tools = new ToolsClass();
            string sainContaint = tools.SanitizeXmlStringBuffer(RowContent);
            XmlNode xml = JsonConvert.DeserializeXmlNode(sainContaint, "records");
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            //Create XmlDoc Object
            xmldoc.LoadXml(xml.InnerXml);
            //Create XML Steam 
            var xmlReader = new XmlNodeReader(xmldoc);
            DataSet dataSet = new DataSet();
            //Load Dataset with Xml
            dataSet.ReadXml(xmlReader);
            //return single table inside of dataset
            var csv = dataSet.Tables[0].ToCSV(",");
            return csv;
        }


        [HttpPost]
        public ActionResult UpdateFollow()
        {

            var Cicrequestresultsfollowed = CicrequestresultsfollowedRepository.Find(Convert.ToInt64(Request.Form["CicRequestResultsFollowedId"]));
            if (Cicrequestresultsfollowed == null)
            {
                TempData["error"] = "Aucun suivi avec l'id " + Request.Form["CicRequestResultsFollowedId"] + " n'a été trouvé.";
                return RedirectToAction("IndexForMailResponses");
            }

            //  Debug.WriteLine("action confirm = " + Request.Form["confirm"]);
            // Debug.WriteLine("action cancel = " + Request.Form["cancel"]);

            //  var StatutInstance = (Statut)System.Enum.Parse(typeof(Statut), statut);
            if (string.IsNullOrEmpty(Request.Form["confirm"])) //action == Cancel
            {
                Cicrequestresultsfollowed.Statut = Statut.A.ToString();
            }
            else if (string.IsNullOrEmpty(Request.Form["cancel"])) //action == confirm
            {
                //l'operation est bien suspecte
                Cicrequestresultsfollowed.Statut = Statut.S.ToString();
                // on lance une requete afin de creer les elements pour pouvoir tirer l'etat Centif
                Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("StartNew confirm");
                CicFollowedPropertiesValuesRepository CicFollowedPropertiesValuesRepository = new CicFollowedPropertiesValuesRepository();
                var NumCompte = "";
                var propNumCmpteInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(Cicrequestresultsfollowed.CicRequestResultsFollowedId, "NumCompte");

                Debug.WriteLine("StartNew propNumCmpteInstance = " + propNumCmpteInstance);

                if (propNumCmpteInstance != null) NumCompte = propNumCmpteInstance.Value;
                Debug.WriteLine("StartNew NumCompte = " + NumCompte);
                runSql(NumCompte);
            });
            }

            CicrequestresultsfollowedRepository.InsertOrUpdate(Cicrequestresultsfollowed);
            CicrequestresultsfollowedRepository.Save();

            //Enregistrement de CicRequestExecution (audit log)
            CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
            var CicRequestExecutionInstance = new CicRequestExecution();
            CicRequestExecutionInstance.CicRequestId = Cicrequestresultsfollowed.CicRequestId;
            CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            CicRequestExecutionInstance.DateAction = DateTime.Now;
            if (string.IsNullOrEmpty(Request.Form["confirm"])) //action == Cancel
            {
                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.FA.ToString();
            }
            else if (string.IsNullOrEmpty(Request.Form["cancel"])) //action == confirm
            {
                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.FC.ToString();
            }
            CicRequestExecutionInstance.DateCreated = DateTime.Now;
            CicRequestExecutionInstance.CicRequestResultsFollowedId = Cicrequestresultsfollowed.CicRequestResultsFollowedId;
            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
            CicrequestExecutionRepository.Save();

            TempData["message"] = "Opération enregistrée avec succés";
            return RedirectToAction("IndexForMailResponses");
        }


        public void Download(long CicMessageMailDocumentsId)
        {
            try
            {
                CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();
                CicMessageMailDocuments document = CicMessageMailDocumentsRepository.Find(CicMessageMailDocumentsId);
                Response.ContentType = document.DocumentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + document.DocumentName);
                Response.OutputStream.Write(document.Document, 0, document.Document.Length);
                Response.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CicRequestResultsFollowedController Download = " + e.StackTrace);
                Response.StatusCode = 404;
            }
        }


        private void runSql(string ribCompte)
        {
            ICicDiversRequestResultsRepository CicDiversRequestResultsRepository = new CicDiversRequestResultsRepository();
            string scriptDirectory = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/sql");
            //   Debug.WriteLine("scriptDirectory = " + scriptDirectory);
            DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                /* Debug.WriteLine("fi.FullName = " + fi.FullName);
                 Debug.WriteLine("fi.Name = " + fi.Name);
                 Debug.WriteLine("fi.Extension = " + fi.Extension);*/
                FileInfo fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cicaudittrailContext"].ConnectionString);
                var cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = script;
                cmd.Parameters.Add("RIB_COMPTE", ribCompte);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        //Si on a des resultats, on supprime d'abord les anciens avant de les enregistrer
                        truncateTable(fi.FullName, ribCompte);

                        //on récupere les resultats de la requete, puis on la garde dans une liste 
                        var model = ToolsClass.ReadToJSON(reader).ToList();
                        Debug.WriteLine("model result  = " + model.Count);
                        foreach (var line in model)
                        {
                            //chaque element de la liste (qui est une ligne de l'ensemble des resultats de la requete executée) est transformée en string, avec ',' comme séparateur, puis stockée dans la table CicRequestResults 
                            //Debug.WriteLine("actual line = " + line);

                            //Insert command
                            string insertsql = "insert into CicDiversRequestResults(Code, Criteria, RowContent, DateCreated) values(:P0,:P1,:P2,:P3)";
                            List<object> parameterList = new List<object>();
                            var filename = fi.Name + "." + fi.Extension;
                            parameterList.Add(fi.Name);
                            parameterList.Add(ribCompte);
                            parameterList.Add(line);
                            parameterList.Add(DateTime.Now);
                            object[] parameters = parameterList.ToArray();

                            int result = CicDiversRequestResultsRepository.GetContext.Database.ExecuteSqlCommand(insertsql, parameters);
                            // int result = conn.Database.ExecuteSqlCommand(insertsql, parameters); 
                        }
                    }
                }
            }
        }


        // methode de suppression de la table CicRequestResults, selon le champ CicRequestId
        private static int truncateTable(string Code, string Criteria)
        {
            try
            {
                ICicDiversRequestResultsRepository CicDiversRequestResultsRepository = new CicDiversRequestResultsRepository();
                using (var ctx = CicDiversRequestResultsRepository.GetContext)
                {
                    string deletesql = "delete from CicDiversRequestResults where Code=:P0 and Criteria=:P1";
                    List<object> parameterList = new List<object>();
                    //   parameterList.Add(CicRequestId);
                    // object[] parameters = parameterList.ToArray();

                    int result = ctx.Database.ExecuteSqlCommand(deletesql, new OracleParameter("P0", Code), new OracleParameter("P1", Criteria));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("truncateTable StackTrace = " + ex.StackTrace);
                return 0;
            }
        }


        public ActionResult GenerateCentifFile(long id)
        {
            try
            {
                ICicRequestResultsFollowedRepository CicrequestresultsfollowedRepository = new CicRequestResultsFollowedRepository();
                string[] status = new string[] { "S", "C" };
                var CicrequestresultsfollowedInstance = CicrequestresultsfollowedRepository.FindByIdAndStatus(id, status);
                if (CicrequestresultsfollowedInstance == null)
                {
                    // Debug.WriteLine("CicrequestresultsfollowedInstance null");
                    TempData["error"] = "Aucun suivi à tirer (statut << Suspect >> ou << Centif tiré >> ) avec l'id " + id + " n'a été trouvé.";
                    return RedirectToAction("IndexForCentif");
                    // Response.StatusCode = 404;
                }
                CicFollowedPropertiesValuesRepository CicFollowedPropertiesValuesRepository = new CicFollowedPropertiesValuesRepository();
                var propNumCmpteInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(CicrequestresultsfollowedInstance.CicRequestResultsFollowedId, "NumCompte");
                //  Debug.WriteLine("propNumCmpteInstance = " + propNumCmpteInstance);

                //Debug.WriteLine("StartNew propNumCmpteInstance = " + propNumCmpteInstance);
                var NumCompte = "";
                if (propNumCmpteInstance != null) NumCompte = propNumCmpteInstance.Value;
                Debug.WriteLine("NumCompte = " + NumCompte);
                ICicDiversRequestResultsRepository CicdiversrequestresultsRepository = new CicDiversRequestResultsRepository();
                //on recherche les données devant etre populées dans le fichier centif
                //clientinfos.sql: le fichier sql executé pour recuperer les infos du client
                CicDiversRequestResults centifData = CicdiversrequestresultsRepository.FindByCodeAndCriteria("clientinfos.sql", NumCompte);
                Debug.WriteLine("centifData = " + centifData);

                string jsonStringCentif = "";
                if (centifData != null) jsonStringCentif = centifData.RowContent;
                string jsonStringOperation = "";
                jsonStringOperation = CicrequestresultsfollowedInstance.RowContent;


                DocX g_document;
                var path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/centif/declaration_soupcon_new_version_exemple.docx");
                Debug.WriteLine("path = " + path);

                FileInfo file = new FileInfo(path);
                //g_document = FillTemplate(DocX.Load(@path), jsonStringCentif, jsonStringOperation);
                g_document = FillTemplate(DocX.Load(path), jsonStringCentif, jsonStringOperation);

                var destinationFileName = "Content/centif/generated_" + CicrequestresultsfollowedInstance.CicRequestResultsFollowedId + ".docx";
                var destinationFile = Path.Combine(HttpRuntime.AppDomainAppPath, destinationFileName);
                // Save all changes made to this template as Invoice_The_Happy_Builder.docx (We don't want to replace InvoiceTemplate.docx).
                g_document.SaveAs(destinationFile);
                Process.Start(destinationFile);

                //update statut CicrequestresultsfollowedInstance
                CicrequestresultsfollowedInstance.Statut = Statut.C.ToString();
                CicrequestresultsfollowedRepository.InsertOrUpdate(CicrequestresultsfollowedInstance);
                CicrequestresultsfollowedRepository.Save();

                //Enregistrement de CicRequestExecution (audit log)
                var CicRequestExecutionInstance = new CicRequestExecution();
                CicRequestExecutionInstance.CicRequestId = CicrequestresultsfollowedInstance.CicRequestId;
                CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                CicRequestExecutionInstance.DateAction = DateTime.Now;
                CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.CT.ToString();
                CicRequestExecutionInstance.DateCreated = DateTime.Now;
                CicRequestExecutionInstance.CicRequestResultsFollowedId = CicrequestresultsfollowedInstance.CicRequestResultsFollowedId;
                CicRequestExecutionInstance.FileName = "generated_" + CicrequestresultsfollowedInstance.CicRequestResultsFollowedId + ".docx";
                CicRequestExecutionInstance.FileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                CicRequestExecutionInstance.CentifFile = System.IO.File.ReadAllBytes(destinationFile);
                CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
                CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                CicrequestExecutionRepository.Save();

                if (centifData != null)
                {
                    TempData["message"] = " Etat généré avec succés";
                }
                else
                {
                    TempData["error"] = "Aucune donnée personnelle de l'agent n'a été trouvée. L'etat a neanmoins été generé mais sans beaucoup de données pré-remplies.";
                }

                return RedirectToAction("IndexForCentif");

                /* 
                Response.ContentType = "docx";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "generated.docx");
                Response.OutputStream.Write(document.Document, 0, document.Document.Length);
                Response.Flush();*/
            }

            catch (IOException e)
            {//TODO rediriger vers la page
                TempData["error"] = "Erreur d'accés aux documents. Veillez à fermer le fichier generated.doc ainsi que le template originale avant de réessayer.";
                Debug.WriteLine("CicRequestResultsFollowedController Download IOException = " + e.StackTrace);
                return RedirectToAction("IndexForCentif");
                // Response.StatusCode = 404;
            }
            catch (Exception e)
            {
                TempData["error"] = "Une erreur inattendue s'est produite. Veuillez contacter le service technique si elle persiste.";
                Debug.WriteLine("CicRequestResultsFollowedController Download Exception = " + e.StackTrace);
                return RedirectToAction("IndexForCentif");
                //  Response.StatusCode = 404;
            }
        }



        private DocX FillTemplate(DocX template, string jsonStringCentif, string jsonStringOperation)
        {
            if (string.IsNullOrWhiteSpace(jsonStringCentif)) jsonStringCentif = "{\"root\": \"root\"}";
            if (string.IsNullOrWhiteSpace(jsonStringOperation)) jsonStringOperation = "{\"root\": \"root\"}";
            //    Debug.WriteLine("jsonStringCentif = " + jsonStringCentif);
            JToken tokenCentif = JObject.Parse(jsonStringCentif);
            JToken tokenOperation = JObject.Parse(jsonStringOperation);
            //   Debug.WriteLine("type_personne = " + (string)tokenCentif.SelectToken("type_personne"));
            // verifier si null ou vide avant de faire un trim    
            //Recup des infos du déclarant (user connecté)
            //TODO recuperer les infos de session
            template.AddCustomProperty(new CustomProperty("DeclarantNom", "DIONE"));
            template.AddCustomProperty(new CustomProperty("DeclarantPrenom", "BATHELEMY  SINK"));
            template.AddCustomProperty(new CustomProperty("DeclarantFonction", "Responsable conformité"));
            template.AddCustomProperty(new CustomProperty("DeclarantTel", "33849 94 33"));
            template.AddCustomProperty(new CustomProperty("DeclarantFax", "33 823 20 05"));
            template.AddCustomProperty(new CustomProperty("DeclarantMail", "BDIONE@cbao.sn"));

            //recup infos declarations
            string format = "dd/MM/yyyy";
            template.AddCustomProperty(new CustomProperty("DeclarationDate", DateTime.Now.ToString(format)));
            template.AddCustomProperty(new CustomProperty("DeclarationDateOperation", string.IsNullOrWhiteSpace((string)tokenOperation.SelectToken("DateComptable")) ? "" : ((string)tokenOperation.SelectToken("DateComptable")).Trim()));
            template.AddCustomProperty(new CustomProperty("DeclarationTypeOperation", string.IsNullOrWhiteSpace((string)tokenOperation.SelectToken("Libelle")) ? "" : ((string)tokenOperation.SelectToken("Libelle")).Trim()));
            template.AddCustomProperty(new CustomProperty("DeclarationMontantTotal", string.IsNullOrWhiteSpace((string)tokenOperation.SelectToken("Montant")) ? "" : ((string)tokenOperation.SelectToken("Montant")).Trim()));
            template.AddCustomProperty(new CustomProperty("DeclarationDevise", "XOF"));
            template.AddCustomProperty(new CustomProperty("DeclarationLieuOperation", string.IsNullOrWhiteSpace((string)tokenOperation.SelectToken("AGENCE")) ? "" : ((string)tokenOperation.SelectToken("AGENCE")).Trim()));
            Debug.WriteLine("type_personne = " + (string)tokenCentif.SelectToken("type_personne"));

            if (!string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("TYPE_PERSONNE")) && ((string)tokenCentif.SelectToken("TYPE_PERSONNE")).Contains("ENTREPRISE"))
            { //#### PERSONNE MORALE
                Debug.WriteLine("MORALE ");

                template.AddCustomProperty(new CustomProperty("DeclarationTypePersonne", "PERSONNE MORALE"));

                template.AddCustomProperty(new CustomProperty("PersonneMoraleRaisonSoc", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("NOM")) ? "" : ((string)tokenCentif.SelectToken("NOM")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleSigle", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("SIGLE")) ? "" : ((string)tokenCentif.SelectToken("SIGLE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleImmatriculation", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("IMMATRICULATION")) ? "" : ((string)tokenCentif.SelectToken("IMMATRICULATION")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleSecteurAct", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("SECTEUR_ACTIVITE")) ? "" : ((string)tokenCentif.SelectToken("SECTEUR_ACTIVITE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleBp", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("BOITE_POSTALE")) ? "" : ((string)tokenCentif.SelectToken("BOITE_POSTALE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleVille", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("VILLE")) ? "" : ((string)tokenCentif.SelectToken("VILLE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleTel", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("TELEPHONE")) ? "" : ((string)tokenCentif.SelectToken("TELEPHONE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleFax", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("FAX")) ? "" : ((string)tokenCentif.SelectToken("FAX")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonneMoraleEmail", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("EMAIL")) ? "" : ((string)tokenCentif.SelectToken("EMAIL")).Trim()));

            }
            else
            { //##### PERSONNE PHYSIQUE
                Debug.WriteLine("PHYSIQUE ");

                template.AddCustomProperty(new CustomProperty("DeclarationTypePersonne", "PERSONNE PHYSIQUE"));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueNom", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("NOM")) ? "" : ((string)tokenCentif.SelectToken("PRENOM")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiquePrenom", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("PRENOM")) ? "" : ((string)tokenCentif.SelectToken("NOM")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueDateNaissance", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("DATE_NAISSANCE")) ? "" : ((string)tokenCentif.SelectToken("DATE_NAISSANCE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueLieuNaissance", (string)tokenCentif.SelectToken("VILLE_NAISSANCE")));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueNationalite", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("NATIONALITE")) ? "" : ((string)tokenCentif.SelectToken("NATIONALITE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueSitFam", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("SITUATION_FAMILLE")) ? "" : ((string)tokenCentif.SelectToken("SITUATION_FAMILLE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueNomConjoint", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("NOM_CONJOINT")) ? "" : ((string)tokenCentif.SelectToken("NOM_CONJOINT")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueActPro", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("PROFESSION")) ? "" : ((string)tokenCentif.SelectToken("PROFESSION")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueEmployeur", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("EMPLOYEUR")) ? "" : ((string)tokenCentif.SelectToken("EMPLOYEUR")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueTypePiece", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("TYPE_DE_PIECE")) ? "" : ((string)tokenCentif.SelectToken("TYPE_DE_PIECE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiquePieceNum", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("NUM_PIECE")) ? "" : ((string)tokenCentif.SelectToken("NUM_PIECE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiquePieceDate", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("DATE_DELIVRANCE")) ? "" : ((string)tokenCentif.SelectToken("DATE_DELIVRANCE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueBp", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("BOITE_POSTALE")) ? "" : ((string)tokenCentif.SelectToken("BOITE_POSTALE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueLocalite", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("ADRESSE_COURRIER")) ? "" : ((string)tokenCentif.SelectToken("ADRESSE_COURRIER")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueTel", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("TELEPHONE")) ? "" : ((string)tokenCentif.SelectToken("TELEPHONE")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueFax", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("FAX")) ? "" : ((string)tokenCentif.SelectToken("FAX")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueEmail", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("EMAIL")) ? "" : ((string)tokenCentif.SelectToken("EMAIL")).Trim()));
                template.AddCustomProperty(new CustomProperty("PersonnePhysiqueDateEntree", string.IsNullOrWhiteSpace((string)tokenCentif.SelectToken("DATE_EMBAUCHE")) ? "" : ((string)tokenCentif.SelectToken("DATE_EMBAUCHE")).Trim()));
            }


            return template;
        }

        //  [HttpPostAttribute]
        public ActionResult SendMailGestionnaireManuel(/*HashSet<CicRequestResultsFollowed> ListErrorMail*/)
        {
            /* Debug.WriteLine("SendMailGestionnaireManuel ListErrorMail = " + ListErrorMail);
             Debug.WriteLine("SendMailGestionnaireManuel ListErrorMail = " + ListErrorMail.Count);*/
            Debug.WriteLine(" TempData[ListErrors] = " + TempData["ListErrors"]);
            var ListErrorMail = TempData["ListErrors"] as HashSet<CicRequestResultsFollowed>;
            Debug.WriteLine(" ListErrorMail = " + ListErrorMail);
            HashSet<CicRequestResultsFollowed> ListErrorMailEmpty = new HashSet<CicRequestResultsFollowed>();
            if (ListErrorMail == null) ListErrorMail = ListErrorMailEmpty;

            foreach (var k in ListErrorMail)
            {
                Debug.WriteLine(" kQueryString = " + k);
            }

            return View(ListErrorMail.AsEnumerable());
        }


        /*   public void generateExcelFile(string RowContent)
           {


               Microsoft.Office.Interop.Excel.Application xlApp;
               Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
               Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
               object misValue = System.Reflection.Missing.Value;
               xlApp = new Microsoft.Office.Interop.Excel.Application();
               xlApp.Visible = false;
               xlWorkBook = (Microsoft.Office.Interop.Excel.Workbook)(xlApp.Workbooks.Add(Missing.Value));
               xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;


               var countline = 0;


               //Remplissage du tableau excel
               //Iteration sur 2 valeurs: la 1ère permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
               for (var a = 0; a <= 1; a++)
               {
                   if (a == 0)
                   {
                       //Reuperation des titres
                       int k = 0;

                       JToken outer = JToken.Parse(RowContent);
                       JObject inner = outer.Value<JObject>();
                       List<string> keys = inner.Properties().Select(p => p.Name).ToList();

                       //Ajout des autres colonnes
                       foreach (string t in keys)
                       {
                           xlWorkSheet.Cells[1, k + 1] = t;
                           k++;
                       }
                       //Les titres sont mis en gras
                       char lastColumn = (char)(65 + keys.Count - 1);
                       xlWorkSheet.get_Range("A1", lastColumn + "1").Font.Bold = true;
                       xlWorkSheet.get_Range("A1",
                       lastColumn + "1").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                   }
                   else
                   {
                       //Recuperation des données
                       var values = new List<object>();

                       JToken outer = JToken.Parse(RowContent);
                       JObject inner = outer.Value<JObject>();
                       List<string> keys = inner.Properties().Select(p => p.Name).ToList();

                       var countinner = 0;
                       foreach (string k in keys)
                       {
                           xlWorkSheet.Cells[countline + 2, countinner + 1] = inner.GetValue(k);
                           countinner++;
                       }
                       countline++;
                   }
               }

               MemoryStream m = new MemoryStream();
               xlWorkBook.SaveToStream();


               xlWorkBook.Close(true, misValue, misValue);
               xlApp.Quit();
               releaseObject(xlWorkSheet);
               releaseObject(xlWorkBook);
               releaseObject(xlApp);


           }


           private void releaseObject(object obj)
           {
               try
               {
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                   obj = null;
               }
               catch
               {
                   obj = null;
                   //MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
               }
               finally
               {
                   GC.Collect();
               }
           }*/
    }
}

