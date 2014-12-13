using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Configuration;
using System.Net;
using System.IO;
using System.Diagnostics;
using cicaudittrail.Models.WsMapping;
using Newtonsoft.Json;
using cicaudittrail.Models;
using System.Text.RegularExpressions;

namespace cicaudittrail.Jobs
{
    public class SyncMailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Debug.WriteLine(" Syncing mail fired= " + DateTime.Now);

            var CommunicationWSUri = ConfigurationManager.AppSettings["GetMailImap"];// recuperation de l'url de l'app de mail
            var appname = ConfigurationManager.AppSettings["Appname"];
            if (CommunicationWSUri != null && appname != null)
            {
                CommunicationWSUri = CommunicationWSUri + appname; // ajout du parametre 'appname' de la methode GET
                Debug.WriteLine("CommunicationWSUri= " + CommunicationWSUri);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CommunicationWSUri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //  request.ProtocolVersion = HttpVersion.Version10;
                request.Timeout = 1000000000;
                request.ReadWriteTimeout = 1000000000;
                request.KeepAlive = false;
                request.ServicePoint.Expect100Continue = false;
                ReadMail(request);

            }

        }


        /*
         * Methode de lecture des mails reçus par une adresse mail.
         * Les credentials de l'adresse mail sont stockés au niveau du fichier Web.config
         * */
        public void ReadMail(HttpWebRequest request)
        {
            string sResponse = "";
            // Assign the response object of HttpWebRequest to a HttpWebResponse variable.
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream streamResponse = response.GetResponseStream())
                {
                    using (StreamReader streamRead = new StreamReader(streamResponse))
                    {
                        sResponse = streamRead.ReadToEnd();

                        Debug.WriteLine("download over ");
                        List<MessagesEntity> mails = JsonConvert.DeserializeObject<List<MessagesEntity>>(sResponse);
                        CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();

                        for (int i = 0; i < mails.Count; i++)
                        {
                            var msg = mails[i];

                            /*    Debug.WriteLine("msg From = " + msg.From.ToString());
                                Debug.WriteLine("msg Subject = " + msg.ObjetMessage);
                                Debug.WriteLine("msg DateMessage = " + msg.DateMessage);
                                Debug.WriteLine("msg attachements = " + msg.Attachements.Count);
                            foreach (var attachment in msg.Attachements)
                            {
                                Debug.WriteLine("  attachements name = " + attachment.fileName);
                                Debug.WriteLine("  attachements type = " + attachment.fileType);
                            }*/

                            //TODO desinstaller MailSystem.net
                            /* L'objet du mail doit être de la forme "Re: Suivi 9: Justificatif operation douteuse".
                             * 9 correspond au CicRequestResultsFollowedId correpsondant au suivi.
                             * */
                            var resultString = ""; int ignoreMe; long CicRequestResultsFollowedId = 0;
                            if (msg.ObjetMessage != null && msg.ObjetMessage.Contains("Suivi ")) resultString = Regex.Match(msg.ObjetMessage, @"\d+").Value;
                            Debug.WriteLine("resultString = " + resultString);

                            bool successfullyParsed = int.TryParse(resultString, out ignoreMe);
                            if (successfullyParsed)
                            {
                                CicRequestResultsFollowedId = Convert.ToInt64(resultString);
                            }
                            Debug.WriteLine("CicRequestResultsFollowedId = " + CicRequestResultsFollowedId);

                            CicMessageMail CicMessageMail = new CicMessageMail();
                            if (checkCicRequestResultsFollowed(CicRequestResultsFollowedId) == true)
                            {
                                Debug.WriteLine("CicRequestResultsFollowedId true = " + CicRequestResultsFollowedId);
                                //on enregistre le mail reçu
                                CicMessageMail.CicRequestResultsFollowedId = CicRequestResultsFollowedId;
                                CicMessageMail.DateMessage = DateTime.Now;
                                CicMessageMail.MessageContent = msg.Message;
                                CicMessageMail.ObjetMessage = msg.ObjetMessage;
                                CicMessageMail.Sens = Sens.I.ToString();
                                CicMessageMail.UserMessage = msg.From;

                                CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                                CicMessageMailRepository.Save();
                                Debug.WriteLine("CicMessageMail saved ");

                                //Save des pieces jointes
                                if (CicMessageMail.CicMessageMailId > 0)
                                {
                                    CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();
                                    foreach (var attachment in msg.Attachements)
                                    {
                                        CicMessageMailDocuments CicMessageMailDocuments = new CicMessageMailDocuments();
                                        CicMessageMailDocuments.CicMessageMailId = CicMessageMail.CicMessageMailId;
                                        CicMessageMailDocuments.DateCreated = DateTime.Now;
                                        CicMessageMailDocuments.DocumentName = attachment.fileName;
                                        CicMessageMailDocuments.DocumentType = attachment.fileType;
                                        if (attachment.file != null)
                                        {
                                            CicMessageMailDocuments.Document = Base64Decode(attachment.file);
                                        }

                                        CicMessageMailDocumentsRepository.InsertOrUpdate(CicMessageMailDocuments);
                                        CicMessageMailDocumentsRepository.Save();
                                        Debug.WriteLine("CicMessageMailDocuments saved ");
                                    }
                                }

                                //Update de CicRequestResultsFollowed: changement de statut
                                CicRequestResultsFollowedRepository CicrequestresultsfollowedRepository = new CicRequestResultsFollowedRepository();

                                var CicRequestFollowedInstance = CicrequestresultsfollowedRepository.Find(CicRequestResultsFollowedId);
                                CicRequestFollowedInstance.Statut = Statut.MR.ToString();
                                CicrequestresultsfollowedRepository.InsertOrUpdate(CicRequestFollowedInstance);
                                CicrequestresultsfollowedRepository.Save();
                                Debug.WriteLine("CicRequestFollowedInstance updated ");
                            }
                        }
                    }
                }
            }
        }

        public static byte[] Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }

        /**
         * Methode qui verifie si un suivi peut recevoir des mails de la part d'un gestionnaire.
         * Pour cela, il doit avoir son statut entre ME: mail envoyé ou MS: mail reçu
         * */
        public bool checkCicRequestResultsFollowed(long CicRequestResultsFollowedId)
        {
            CicRequestResultsFollowedRepository CicRequestResultsFollowedRepository = new CicRequestResultsFollowedRepository();
            //On verifie d'abord que l'id reçu dans l'objet correspond bien à un suivi
            var CicRequestResultsFollowedInstance = CicRequestResultsFollowedRepository.Find(CicRequestResultsFollowedId);
            if (CicRequestResultsFollowedInstance != null && new List<String>() { "ME", "MR" }.Contains(CicRequestResultsFollowedInstance.Statut))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}