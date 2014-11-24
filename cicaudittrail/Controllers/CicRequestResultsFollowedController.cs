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
                TempData["error"] = "Aucune requ�te avec l'id " + id + " n'a �t� trouv�e.";
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
                TempData["error"] = "Veuillez v�rifier vos suivis. Aucun enregistrement avec l'id " + id + " n'a �t� trouv�e.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(CicrequestresultsfollowedInstance.RowContent))
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance RowContent empty");
                TempData["error"] = "Veuillez v�rifier la requ�te � ex�cuter. Aucune requ�te avec l'id " + id + " n'a �t� trouv�e.";
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

        public ActionResult DetailsPrintMessages(long id)
        {
            var model = new List<object[]>();
            var listResults = new List<CicRequestResultsFollowed>();
            var CicrequestresultsfollowedInstance = CicrequestresultsfollowedRepository.Find(id);
            if (CicrequestresultsfollowedInstance == null)
            {
                // Debug.WriteLine("CicrequestresultsfollowedInstance null");
                TempData["error"] = "Veuillez v�rifier vos suivis. Aucun enregistrement avec l'id " + id + " n'a �t� trouv�e.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(CicrequestresultsfollowedInstance.RowContent))
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance RowContent empty");
                TempData["error"] = "Veuillez v�rifier la requ�te � ex�cuter. Aucune requ�te avec l'id " + id + " n'a �t� trouv�e.";
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
                TempData["error"] = "Veuillez v�rifier vos suivis. Aucun enregistrement avec l'id " + id + " n'a �t� trouv�e.";
                //return RedirectToAction("Index");
                return PartialView("_DetailsFollowed", model);
            }

            if (string.IsNullOrEmpty(CicrequestresultsfollowedInstance.RowContent))
            {
                //  Debug.WriteLine("CicrequestresultsfollowedInstance RowContent empty");
                TempData["error"] = "Veuillez v�rifier la requ�te � ex�cuter. Aucune requ�te avec l'id " + id + " n'a �t� trouv�e.";
                //return View(CicrequestresultsfollowedInstance);
                return PartialView("_DetailsFollowed", model);
            }
            listResults.Add(CicrequestresultsfollowedInstance);
            model = Read(listResults).ToList();

            return PartialView("_DetailsFollowed", model);
            //return View(model);
        }

        //Methode de lecture des donn�es du champ RowContent.
        //Ce champ contient les resultats des requetes parametr�es. Ils sont en JSON, de la forme:
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
  "NATIONALIT�": "S�n�galaise                   ",
  "PROFESSION": null,
  "MvtsAuCredit": 53774202.0,
  "MvtsAuDebit": 54270680.0
}
         * */
        //Cette methode recupere plusieurs lignes de cette forme et les presente sous forme de tableau dans la vue Index, 
        //avec les cl�s en entete, et les valeurs dans le corps.
        // NB: on aurait pu diminuer le couplage, en envoyant le JSON, mais on a besoin de l'id du CicRequestResults, so...
        private static IEnumerable<object[]> Read(List<CicRequestResultsFollowed> resultsList)
        {
            //Iteration sur 2 valeurs: la 1�re permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
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
         * Elle re�oit des id de CicRequestResultsFollowed. Elle recupere le messageTemplate associ�, afin d'avoir le contenu du message mail, fait un binding afin de remplacer les variables contenues dans le messageTemplate, puis cr�e un enregistrement de CicMessageMail.
        */
        // POST: /CicRequestResultsFollowed/PopulateBody

        [HttpPost]
        public ActionResult PopulateBody(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            //Debug.WriteLine("Request.Form = " + Request.Form);
            CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();
            CicRequestExecutionRepository CicrequestExecutionRepository = new CicRequestExecutionRepository();
            var nbre = 0;
            //Liste des mails � envoyer
            HashSet<CicMessageMail> ListCicMessageMail = new HashSet<CicMessageMail>();
            //Liste des erreurs d'envois
            HashSet<CicRequestResultsFollowed> ListErrorMail = new HashSet<CicRequestResultsFollowed>();

            foreach (var k in Request.Form.AllKeys)
            {
                if (k.ToString().StartsWith("coche."))
                {
                    var resultId = k.Split('.')[1];
                    //si case coch�e <==> si on veut envoyer msg pour la ligne en cours
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

                            var propEmailInstance = CicFollowedPropertiesValuesRepository.FindByRequestFollowedAndProperty(CicRequestFollowedInstance.CicRequestResultsFollowedId, "Email"); // TODO verifier validit� mail regex 
                            if (propEmailInstance != null) email = propEmailInstance.Value;

                            //on remplit les variables du message template
                            map.Add("#{NomGestionnaire}", NomGestionnaire);
                            ToolsClass toolsClass = new ToolsClass();
                            var objet = "Suivi " + CicRequestFollowedInstance.CicRequestResultsFollowedId + ": " + CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ObjetMail;
                            var bodyMessage = CicRequestFollowedInstance.CicRequest.CicMessageTemplate.ContenuMail;
                            var CicMessageMail = new CicMessageMail();
                            CicMessageMail.CicRequestResultsFollowedId = CicRequestFollowedInstance.CicRequestResultsFollowedId;
                            CicMessageMail.DateMessage = DateTime.Now;
                            CicMessageMail.MessageContent = toolsClass.generateBodyMessage(map, bodyMessage);
                            CicMessageMail.ObjetMessage = objet;
                            CicMessageMail.Email = email;
                            CicMessageMail.UserMessage = "admin"; //TODO recuperer le user connect�
                            CicMessageMail.Sens = Sens.O.ToString();
                            CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                            CicMessageMailRepository.Save();
                            // on envoie le mail que si l'adresse mail du gestionnaire est renseign�e
                            if (email != null && email != "")
                                ListCicMessageMail.Add(CicMessageMail);

                            //TODO enregistrer la piece jointe: le json en xls
                            //Generation fichier excel (csv) 
                            var RowContentcsv = generateCSV(CicRequestFollowedInstance.RowContent);
                            //  Debug.WriteLine("RowContentcsv = " + RowContentcsv);
                            // transform en byte
                            byte[] RowContentbytes = Encoding.Default.GetBytes(RowContentcsv);
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
                            // on ne flag le suivi que si le adresse mail du gestionnaire est renseign�e
                            // sinon on l'enregistre dans la liste des erreurs
                            if (email != null && email != "")
                            {
                                CicRequestFollowedInstance.Statut = Statut.ME.ToString();
                                CicrequestresultsfollowedRepository.InsertOrUpdate(CicRequestFollowedInstance);
                                CicrequestresultsfollowedRepository.Save();
                            }
                            else ListErrorMail.Add(CicRequestFollowedInstance);

                            //Enregistrement de CicRequestExecution (audit log)
                            var CicRequestExecutionInstance = new CicRequestExecution();
                            CicRequestExecutionInstance.CicRequestId = CicRequestFollowedInstance.CicRequestId;
                            CicRequestExecutionInstance.UserAction = "admin"; //TODO mettre le user connect�
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
                    CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();

                    ArrayList mailArray = new ArrayList();
                    mailArray.Add(mail.ConvertToMessageEntity());
                    var MessageMailDocumentsList = CicMessageMailDocumentsRepository.FindAllByCicMessageMail(mail.CicMessageMailId);
                    foreach (CicMessageMailDocuments msgdoc in MessageMailDocumentsList)
                    {
                        mailArray.Add(msgdoc.ConvertToMessageEntityPJ());
                    }

                    var rstl = MailingClass.SendEmail(mailArray);
                    Debug.WriteLine("MailingClass rstl = " + rstl);
                    if (rstl > 0)
                    {
                        mail.SMTPProviderMessageId = rstl;
                        CicMessageMailRepository.InsertOrUpdate(mail);
                        CicMessageMailRepository.Save();

                    }
                }
            });
            if (ListErrorMail.Count > 0)
            {
                Debug.WriteLine("ListErrorMail = " + ListErrorMail.Count);

                // TODO return vue erreur liste erreurs
                TempData["message"] = nbre + " message(s) a (ont) �t� envoy� avec succ�s";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = nbre + " message(s) a (ont) �t� envoy� avec succ�s";
                return RedirectToAction("Index");
            }
        }

        private string generateCSV(string RowContent)
        {
            XmlNode xml = JsonConvert.DeserializeXmlNode(RowContent, "records");
            XmlDocument xmldoc = new XmlDocument();
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
                TempData["error"] = "Aucun suivi avec l'id " + Request.Form["CicRequestResultsFollowedId"] + " n'a �t� trouv�.";
                return RedirectToAction("IndexForMailResponses");
            }

            Debug.WriteLine("action confirm = " + Request.Form["confirm"]);
            Debug.WriteLine("action cancel = " + Request.Form["cancel"]);

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
            CicRequestExecutionInstance.UserAction = "admin"; //TODO mettre le user connect�
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

            TempData["message"] = "Op�ration enregistr�e avec succ�s";
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
            Debug.WriteLine("scriptDirectory = " + scriptDirectory);
            DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                Debug.WriteLine("fi.FullName = " + fi.FullName);
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

                        //on r�cupere les resultats de la requete, puis on la garde dans une liste 
                        var model = ToolsClass.ReadToJSON(reader).ToList();
                        Debug.WriteLine("model result  = " + model.Count);
                        foreach (var line in model)
                        {
                            //chaque element de la liste (qui est une ligne de l'ensemble des resultats de la requete execut�e) est transform�e en string, avec ',' comme s�parateur, puis stock�e dans la table CicRequestResults 
                            //Debug.WriteLine("actual line = " + line);

                            //Insert command
                            string insertsql = "insert into CicDiversRequestResults(Code, Criteria, RowContent, DateCreated) values(:P0,:P1,:P2,:P3)";
                            List<object> parameterList = new List<object>();
                            parameterList.Add(fi.FullName);
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



        private DocX FillTemplate(DocX template, String jsonStringCentif, String jsonStringOperation)
        {
            JToken tokenCentif = JObject.Parse(jsonStringCentif);
            JToken tokenOperation = JObject.Parse(jsonStringOperation);
            //Recup des infos du d�clarant (user connect�)
            //TODO recuperer les infos de session
            template.AddCustomProperty(new CustomProperty("DeclarantNom", "DIONE"));
            template.AddCustomProperty(new CustomProperty("DeclarantPrenom", "BATHELEMY  SINK"));
            template.AddCustomProperty(new CustomProperty("DeclarantFonction", "Responsable conformit�"));
            template.AddCustomProperty(new CustomProperty("DeclarantTel", "33849 94 33"));
            template.AddCustomProperty(new CustomProperty("DeclarantFax", "33 823 20 05"));
            template.AddCustomProperty(new CustomProperty("DeclarantMail", "BDIONE@cbao.sn"));

            //recup infos declarations
            template.AddCustomProperty(new CustomProperty("DeclarationDate", DateTime.Now));
            template.AddCustomProperty(new CustomProperty("DeclarationDateOperation", (string)tokenOperation.SelectToken("DateComptable")));
            template.AddCustomProperty(new CustomProperty("DeclarationTypeOperation", (string)tokenOperation.SelectToken("Libelle")));
            template.AddCustomProperty(new CustomProperty("DeclarationMontantTotal", (string)tokenOperation.SelectToken("Montant")));
            template.AddCustomProperty(new CustomProperty("DeclarationDevise", "XOF"));
            template.AddCustomProperty(new CustomProperty("DeclarationLieuOperation", (string)tokenOperation.SelectToken("AGENCE")));
            template.AddCustomProperty(new CustomProperty("DeclarationMontantTotal", (string)tokenOperation.SelectToken("Montant")));
            template.AddCustomProperty(new CustomProperty("DeclarationMontantTotal", (string)tokenOperation.SelectToken("Montant")));
            template.AddCustomProperty(new CustomProperty("DeclarationMontantTotal", (string)tokenOperation.SelectToken("Montant"))); 


            template.AddCustomProperty(new CustomProperty("First_Name", "Andrea"));
            template.AddCustomProperty(new CustomProperty("Last_Name", "Regoli"));
            template.AddCustomProperty(new CustomProperty("site", "http://www.andrearegoli.it"));

            return template;
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
               //Iteration sur 2 valeurs: la 1�re permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
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
                       //Recuperation des donn�es
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

