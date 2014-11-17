using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using Newtonsoft.Json;
using ActiveUp.Net.Mail;
using cicaudittrail.Models;
using System.Diagnostics;
using System.Net;
using System.IO;
using cicaudittrail.Models.WsMapping;
//using OpenPop.Pop3;

namespace cicaudittrail.Src
{
    public class MailingClass
    {
        /*
         * Methode d'envoi de mails.
         * Les credentials de l'adresse mail sont stockés au niveau du fichier Web.config
         * */
        public void SendEmail(string recepientEmail, string subject, string body)
        {

            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.UseDefaultCredentials = false;

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];

                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        }

        /*
         * Methode de lecture des mails reçus par une adresse mail.
         * Les credentials de l'adresse mail sont stockés au niveau du fichier Web.config
         * */
        public void ReadMail()
        {

            var CommunicationWSUri = ConfigurationManager.AppSettings["GetMailImap"];// recuperation de l'url de l'app de mail
            var appname = ConfigurationManager.AppSettings["Appname"];
            if (CommunicationWSUri != null && appname != null)
            {
                string sResponse = "";
                CommunicationWSUri = CommunicationWSUri + appname; // ajout du parametre 'appname' de la methode GET
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CommunicationWSUri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //  request.ProtocolVersion = HttpVersion.Version10;
                request.Timeout = 1000000000;
                request.ReadWriteTimeout = 1000000000;
                // request.ContinueT = 1000000000;
                request.KeepAlive = false;
                request.ServicePoint.Expect100Continue = false;
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

                            for (int i = 0; i < mails.Count; i++) // Loop through List with for
                            {
                                var msg = mails[i];

                                Debug.WriteLine("msg From = " + msg.From.ToString());
                                Debug.WriteLine("msg Subject = " + msg.ObjetMessage);
                                Debug.WriteLine("msg DateMessage = " + msg.DateMessage);
                                Debug.WriteLine("msg attachements = " + msg.Attachements.Count);
                                foreach (var attachment in msg.Attachements)
                                {
                                    Debug.WriteLine("  attachements name = " + attachment.fileName);
                                    Debug.WriteLine("  attachements type = " + attachment.fileType);
                                }

                                //TODO msg Subject = Fwd: Suivi 9: Justificatif operation douteuse: recuperer le CicRequestResultsFollowedId selon le Subject

                               /* CicMessageMail CicMessageMail = new CicMessageMail();
                                CicMessageMail.DateMessage = DateTime.Now;
                                CicMessageMail.MessageContent = msg.Message;
                                CicMessageMail.ObjetMessage = msg.ObjetMessage;
                                CicMessageMail.Sens = Sens.I.ToString();
                                CicMessageMail.UserMessage = msg.From;

                                //TODO recup CicRequestResultsFollowedId selon le msg.Subject
                                CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                                CicMessageMailRepository.Save();

                                //Gestion des pieces jointes
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
                                }*/
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

    }
}