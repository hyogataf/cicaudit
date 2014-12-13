using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cicaudittrail.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO;
using cicaudittrail.Src;
using Newtonsoft.Json;

namespace cicaudittrail.Controllers
{
    public class CicRequestResultsController : Controller
    {
        private readonly ICicRequestRepository CicrequestRepository;
        private readonly ICicRequestResultsRepository CicrequestresultsRepository;
        private readonly ICicRequestExecutionRepository CicrequestExecutionRepository;
        private readonly ICicRequestResultsFollowedRepository CicrequestResultsFollowedRepository;
        public System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();

        // If you are using Dependency Injection, you can delete the following constructor
        public CicRequestResultsController()
            : this(new CicRequestRepository(), new CicRequestResultsRepository(), new CicRequestExecutionRepository(), new CicRequestResultsFollowedRepository())
        {
        }

        public CicRequestResultsController(ICicRequestRepository CicrequestRepository, ICicRequestResultsRepository CicrequestresultsRepository, ICicRequestExecutionRepository CicrequestExecutionRepository, ICicRequestResultsFollowedRepository CicrequestResultsFollowedRepository)
        {
            this.CicrequestRepository = CicrequestRepository;
            this.CicrequestresultsRepository = CicrequestresultsRepository;
            this.CicrequestExecutionRepository = CicrequestExecutionRepository;
            this.CicrequestResultsFollowedRepository = CicrequestResultsFollowedRepository;
        }

        //
        // GET: /CicRequestResults/

        public ViewResult Index()
        {
            return View(CicrequestresultsRepository.All);
        }

        //
        // GET: /CicRequestResults/Details/5

        public ViewResult Details(long id)
        {
            return View(CicrequestresultsRepository.Find(id));
        }


        //
        // GET: /CicRequestResults/DetailsByRequest/5

        public ViewResult DetailsByRequest(long id)
        {
            CicRequest CicRequest = CicrequestRepository.Find(id);
            var model = new List<object[]>();
            if (CicRequest == null)
            {
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête avec l'id " + id + " n'a été trouvée.";
                return View(model);
            }

            if (string.IsNullOrEmpty(CicRequest.Request))
            {
                TempData["error"] = "Veuillez vérifier la requête à exécuter. Aucune requête SQL detectée";
                return View(model);
            }

            // Debug.WriteLine("DateTime.Today = " + DateTime.Today);
            var listResults = CicrequestresultsRepository.FindAllByRequestAndDate(id, DateTime.Today).ToList<CicRequestResults>();// TODO remettre ça
            // var listResults = CicrequestresultsRepository.FindAllByRequestAndDate(id, DateTime.Today.AddDays(-1)).ToList<CicRequestResults>();
            //  var listResults = CicrequestresultsRepository.FindAllByRequest(id).ToList<CicRequestResults>();
            //var test = Read(listResults).ToList();
            if (listResults.Any())
            {
                model = ReadJSON(listResults).ToList();
            }

            ViewData["CicRequest.Code"] = CicRequest.Code;
            ViewData["CicRequest.Libelle"] = CicRequest.Libelle;
            ViewData["CicRequest.Id"] = CicRequest.CicRequestId;

            //Enregistrement de CicRequestExecution
            var CicRequestExecutionInstance = new CicRequestExecution();
            CicRequestExecutionInstance.CicRequestId = id;
            CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
            CicRequestExecutionInstance.DateAction = DateTime.Now;
            CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.E.ToString();
            CicRequestExecutionInstance.DateCreated = DateTime.Now;
            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
            CicrequestExecutionRepository.Save();

            return View(model);
        }

        private static IEnumerable<object[]> Read(List<CicRequestResults> resultsList)
        {
            foreach (var line in resultsList)
            {
                List<string> result = line.RowContent.Split(new string[] { "|#|" }, StringSplitOptions.None).ToList();
                var values = new List<object>();
                values.Add(line.CicRequestResultsId);
                foreach (var item in result)
                {
                    values.Add(item);
                }
                yield return values.ToArray();
            }
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
        private static IEnumerable<object[]> ReadJSON(List<CicRequestResults> resultsList)
        {
            //Iteration sur 2 valeurs: la 1ère permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
            for (var i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    var values = new List<object>();
                    var line = resultsList[0];
                    values.Add(line.CicRequestResultsId);

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
                        values.Add(line.CicRequestResultsId);

                        JToken outer = JToken.Parse(line.RowContent);
                        JObject inner = outer.Value<JObject>();
                        List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                        /*   for (int i = 0; i <= keys.Count; i++) {
                               if (i == 0) {
                                   Debug.WriteLine("key = " + keys[i] + " , value= " + inner.GetValue(keys[i]));
                                   values.Add(keys[i]);

                               }
                           }*/

                        foreach (string k in keys)
                        {
                            values.Add(inner.GetValue(k));
                        }
                        yield return values.ToArray();
                    }
                }
            }
        }


        //Methode de recuperation des suivis effectués sur les resultats de la requete
        [HttpPost]
        public ActionResult FollowCicRequestResult()
        {
            var nbre = 0;
            foreach (var k in Request.Form.AllKeys)
            {
                if (k.ToString().StartsWith("result."))
                {
                    var resultId = k.Split('.')[1];

                    //si case cochée <==> si on veut suivre la ligne en cours
                    if (Request.Form.GetValues(k.ToString())[0] == "true")
                    {
                        CicRequestResults CicRequestResultsInstance = CicrequestresultsRepository.Find(Convert.ToInt64(resultId));
                        if (CicRequestResultsInstance != null)
                        {
                            var CicRequestResultsFollowed = new CicRequestResultsFollowed();
                            CicRequestResultsFollowed.CicRequestId = CicRequestResultsInstance.CicRequestId;
                            CicRequestResultsFollowed.RowContent = CicRequestResultsInstance.RowContent;
                            CicRequestResultsFollowed.DateCreated = DateTime.Now;
                            CicRequestResultsFollowed.Statut = Statut.E.ToString();
                            CicRequestResultsFollowed.UserCreated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                            CicRequestResultsFollowed.Comments = Request.Form["Comments" + resultId];
                            CicrequestResultsFollowedRepository.InsertOrUpdate(CicRequestResultsFollowed);
                            CicrequestResultsFollowedRepository.Save();

                            //Recuperation des properties de CicRequest 

                            if (CicRequestResultsInstance.CicRequest.Properties != null && CicRequestResultsInstance.CicRequest.Properties != "")
                            {
                                var listProperties = CicRequestResultsInstance.CicRequest.Properties.Split(',');

                                Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                var dictionaryIgnoreCase = JsonConvert.DeserializeObject<Dictionary<string, string>>(CicRequestResultsInstance.RowContent);
                                foreach (var el in dictionaryIgnoreCase)
                                {
                                    dictionary.Add(el.Key, el.Value);
                                }

                                foreach (var proplocal in listProperties)
                                {
                                    var prop = proplocal.Trim();

                                    if (prop != null)
                                    {
                                        if (dictionary.ContainsKey(prop) && !string.IsNullOrWhiteSpace(dictionary[prop]))
                                        {
                                            // Debug.WriteLine("prop = " + prop + ", value = " + dictionary[prop].Trim());
                                            CicFollowedPropertiesValuesRepository CicRequestPropValueRepository = new CicFollowedPropertiesValuesRepository();
                                            CicFollowedPropertiesValues CicRequestPropValue = new CicFollowedPropertiesValues();
                                            CicRequestPropValue.CicRequestResultsFollowedId = CicRequestResultsFollowed.CicRequestResultsFollowedId;
                                            CicRequestPropValue.Property = prop;
                                            CicRequestPropValue.Value = dictionary[prop].Trim();
                                            CicRequestPropValue.DateCreated = DateTime.Now;
                                            CicRequestPropValueRepository.InsertOrUpdate(CicRequestPropValue);
                                            CicRequestPropValueRepository.Save();
                                        }
                                    }
                                }
                            }

                            //Enregistrement de CicRequestExecution
                            var CicRequestExecutionInstance = new CicRequestExecution();
                            CicRequestExecutionInstance.CicRequestId = CicRequestResultsInstance.CicRequestId;
                            CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                            CicRequestExecutionInstance.DateAction = DateTime.Now;
                            CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.F.ToString();
                            CicRequestExecutionInstance.DateCreated = DateTime.Now;
                            CicRequestExecutionInstance.CicRequestResultsFollowedId = CicRequestResultsFollowed.CicRequestResultsFollowedId;
                            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                            CicrequestExecutionRepository.Save();
                            nbre++;
                        }
                    }
                }
                //Debug.WriteLine("result Form AllKeys= " + k);
            }
            TempData["message"] = nbre + " élements a (ont) été marqués. Leur suivi a été enregistré avec succés";

            return RedirectToAction("IndexByRequest", "CicRequestResultsFollowed", new { id = Request.Form["CicRequestId"] });

        }

        //
        // GET: /CicRequestResults/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View();
        }

        //
        // POST: /CicRequestResults/Create

        [HttpPost]
        public ActionResult Create(CicRequestResults Cicrequestresults)
        {
            if (ModelState.IsValid)
            {
                CicrequestresultsRepository.InsertOrUpdate(Cicrequestresults);
                CicrequestresultsRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRequest = CicrequestRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequestResults/Edit/5

        public ActionResult Edit(long id)
        {
            ViewBag.PossibleCicRequest = CicrequestRepository.All;
            return View(CicrequestresultsRepository.Find(id));
        }

        //
        // POST: /CicRequestResults/Edit/5

        [HttpPost]
        public ActionResult Edit(CicRequestResults Cicrequestresults)
        {
            if (ModelState.IsValid)
            {
                CicrequestresultsRepository.InsertOrUpdate(Cicrequestresults);
                CicrequestresultsRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCicRequest = CicrequestRepository.All;
                return View();
            }
        }

        //
        // GET: /CicRequestResults/Delete/5

        public ActionResult Delete(long id)
        {
            return View(CicrequestresultsRepository.Find(id));
        }

        //
        // POST: /CicRequestResults/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            CicrequestresultsRepository.Delete(id);
            CicrequestresultsRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CicrequestRepository.Dispose();
                CicrequestresultsRepository.Dispose();
            }
            base.Dispose(disposing);
        }



        //
        // GET: /CicRequestResults/Create

        public ActionResult ExportToExcel(long id)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;
            xlWorkBook = (Microsoft.Office.Interop.Excel.Workbook)(xlApp.Workbooks.Add(Missing.Value));
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;

            var listResults = CicrequestresultsRepository.FindAllByRequestAndDate(id, DateTime.Now).ToList<CicRequestResults>();
            if (listResults.Any())
            {
                var countline = 0;


                //Remplissage du tableau excel
                //Iteration sur 2 valeurs: la 1ère permet de recuperer les titres (pour les mettre en entete), la 2nde les valeurs
                for (var a = 0; a <= 1; a++)
                {
                    if (a == 0)
                    {
                        //Reuperation des titres
                        int k = 2; // iteration sur les colonnes (titres). On start à 2 pour ajouter en dur "Id" et "Commentaires"
                        var line = listResults[0];
                        JToken outer = JToken.Parse(line.RowContent);
                        JObject inner = outer.Value<JObject>();
                        List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                        //Ajout de 2 colonnes pour afficher les id et un champ Commentaires
                        xlWorkSheet.Cells[1, 1] = "Id"; // ajout de "Id"
                        xlWorkSheet.Cells[1, 2] = "Commentaires"; // ajout de "Commentaires"
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
                        foreach (var line in listResults)
                        {
                            //Recuperation des données
                            var values = new List<object>();

                            JToken outer = JToken.Parse(line.RowContent);
                            JObject inner = outer.Value<JObject>();
                            List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                            //Renseignement de l'id
                            xlWorkSheet.Cells[countline + 2, 1] = line.CicRequestResultsId;
                            var countinner = 2;
                            foreach (string k in keys)
                            {
                                xlWorkSheet.Cells[countline + 2, countinner + 1] = inner.GetValue(k);
                                countinner++;
                            }
                            countline++;
                        }
                    }
                }

                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                TempData["message"] = "L'export vers excel s'est déroulé avec succés.";
                return RedirectToAction("DetailsByRequest", new { id = id });
            }
            else
            {
                TempData["error"] = "La requête envoyée n'a retourné aucun résultat à exporter.";
                return RedirectToAction("DetailsByRequest", new { id = id });
            }
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
        }


        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult ManualFollowCicRequestResult(HttpPostedFileBase file)
        {
            // Verify that the user selected a file 
            var count = 0;
            if (Request.Form["CicRequestId"] == null || Request.Form["CicRequestId"] == "")
            {
                TempData["error"] = "Des données manquantes ont été constatées, l'operation ne peut donc être poursuivie.";
                return RedirectToAction("Index", "CicRequest");
            }
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                //Debug.WriteLine("fileName= " + fileName);
                if (!(fileName.EndsWith(".xls")) && !(fileName.EndsWith(".xlsx")))
                {
                    TempData["error"] = "Uniquement les fichiers excels sont acceptés (format xls ou xlsx). Veuillez verifier votre upload";
                    return RedirectToAction("DetailsByRequest", new { id = Request.Form["CicRequestId"] });
                }

                // store the file inside ~/App_Data/uploads folder
                //string webCurrentDirectory = HttpRunTime.AppDomainAppPath; 
                var path = Path.Combine(Server.MapPath("~/Content/uploads"), fileName);
                file.SaveAs(path);

                //Recuperation des données renseignées sous forme de list
                var suiviList = getExcelSuiviMappings(path);
                foreach (var line in suiviList)
                {
                    Debug.WriteLine("debut suivi = " + " {0} - {1}", line.Id, line.Comments);
                    int n;
                    bool isNumeric = int.TryParse(line.Id, out n);
                    if (isNumeric)
                    {
                        CicRequestResults CicRequestResultsInstance = CicrequestresultsRepository.Find(Convert.ToInt64(line.Id));
                        if (CicRequestResultsInstance != null)
                        {
                            var CicRequestResultsFollowed = new CicRequestResultsFollowed();
                            CicRequestResultsFollowed.CicRequestId = CicRequestResultsInstance.CicRequestId;
                            CicRequestResultsFollowed.RowContent = CicRequestResultsInstance.RowContent;
                            CicRequestResultsFollowed.DateCreated = DateTime.Now;
                            CicRequestResultsFollowed.Statut = Statut.E.ToString();
                            CicRequestResultsFollowed.UserCreated = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                            CicRequestResultsFollowed.Comments = line.Comments;
                            CicrequestResultsFollowedRepository.InsertOrUpdate(CicRequestResultsFollowed);
                            CicrequestResultsFollowedRepository.Save();

                            //Recuperation des properties de CicRequest 

                            if (CicRequestResultsInstance.CicRequest.Properties != null && CicRequestResultsInstance.CicRequest.Properties != "")
                            {
                                Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                var dictionaryIgnoreCase = JsonConvert.DeserializeObject<Dictionary<string, string>>(CicRequestResultsInstance.RowContent);
                                foreach (var el in dictionaryIgnoreCase)
                                {
                                    dictionary.Add(el.Key, el.Value);
                                }
                                var listProperties = CicRequestResultsInstance.CicRequest.Properties.Split(',');
                                foreach (var proplocal in listProperties)
                                {
                                    var prop = proplocal.Trim();

                                    if (prop != null)
                                    {
                                        if (dictionary.ContainsKey(prop))
                                        //  if (listProps.Contains(prop, StringComparer.OrdinalIgnoreCase))
                                        {
                                            CicFollowedPropertiesValuesRepository CicRequestPropValueRepository = new CicFollowedPropertiesValuesRepository();
                                            CicFollowedPropertiesValues CicRequestPropValue = new CicFollowedPropertiesValues();
                                            CicRequestPropValue.CicRequestResultsFollowedId = CicRequestResultsFollowed.CicRequestResultsFollowedId;
                                            CicRequestPropValue.Property = prop;
                                            CicRequestPropValue.Value = dictionary[prop].Trim();
                                            CicRequestPropValue.DateCreated = DateTime.Now;
                                            CicRequestPropValueRepository.InsertOrUpdate(CicRequestPropValue);
                                            CicRequestPropValueRepository.Save();
                                        }
                                    }
                                }
                            }

                            //Enregistrement de CicRequestExecution
                            var CicRequestExecutionInstance = new CicRequestExecution();
                            CicRequestExecutionInstance.CicRequestId = CicRequestResultsInstance.CicRequestId;
                            CicRequestExecutionInstance.UserAction = Session["CurrentUser"] == null ? CurrentUser.Email : Session["CurrentUser"].ToString();
                            CicRequestExecutionInstance.DateAction = DateTime.Now;
                            CicRequestExecutionInstance.Action = cicaudittrail.Models.Action.F.ToString();
                            CicRequestExecutionInstance.DateCreated = DateTime.Now;
                            CicRequestExecutionInstance.CicRequestResultsFollowedId = CicRequestResultsFollowed.CicRequestResultsFollowedId;
                            CicrequestExecutionRepository.InsertOrUpdate(CicRequestExecutionInstance);
                            CicrequestExecutionRepository.Save();
                            count++;
                        }
                    }
                }
            }
            else
            {
                TempData["error"] = "Aucun fichier n'a été detecté. Veuillez verifier votre upload";
            }
            // redirect back to the index action to show the form once again 
            TempData["message"] = count + " élements a (ont) été marqués. Leur suivi a été enregistré avec succés";

            return RedirectToAction("IndexByRequest", "CicRequestResultsFollowed", new { id = Request.Form["CicRequestId"] });
        }



        IEnumerable<ExcelSuiviMapping> getExcelSuiviMappings(string path)
        {
            List<ExcelSuiviMapping> suiviMappingList = new List<ExcelSuiviMapping>();

            var excelData = new ExcelData(path);
            var suivis = excelData.getData("Feuil1");
            foreach (var row in suivis)
            {
                var suivi = new ExcelSuiviMapping()
                {
                    Id = row["Id"].ToString(),
                    Comments = row["Commentaires"].ToString()
                };
                suiviMappingList.Add(suivi);
            }
            return suiviMappingList;
        }
    }
}

