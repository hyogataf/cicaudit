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

                SmtpClient smtp = new SmtpClient();
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

            var CommunicationWSUri = ConfigurationManager.AppSettings["GetMailImap"];
            var appname = ConfigurationManager.AppSettings["Appname"];
            if (CommunicationWSUri != null && appname != null)
            {
                CommunicationWSUri = CommunicationWSUri + appname; // ajout du parametre 'appname' de la methode GET
                System.Net.WebClient webClient = new System.Net.WebClient();
                string download = webClient.DownloadString(CommunicationWSUri);

                List<Message> mails = JsonConvert.DeserializeObject<List<Message>>(download);
                CicMessageMailRepository CicMessageMailRepository = new CicMessageMailRepository();

                for (int i = 0; i < mails.Count; i++) // Loop through List with for
                {
                    var msg = mails[i];

                    Debug.WriteLine("msg From = " + msg.From.ToString());
                    Debug.WriteLine("msg Sender = " + msg.Sender.ToString());
                    Debug.WriteLine("msg Subject = " + msg.Subject);

                    CicMessageMail CicMessageMail = new CicMessageMail();
                    CicMessageMail.DateMessage = DateTime.Now;
                    CicMessageMail.MessageContent = msg.BodyHtml.Text;
                    CicMessageMail.ObjetMessage = msg.Subject;
                    CicMessageMail.Sens = Sens.I.ToString();
                    CicMessageMail.UserMessage = msg.From.Name + "<" + msg.From.Email + ">";

                    //TODO recup CicRequestResultsFollowedId selon le msg.Subject
                    CicMessageMailRepository.InsertOrUpdate(CicMessageMail);
                    CicMessageMailRepository.Save();

                    //Gestion des pieces jointes
                    CicMessageMailDocumentsRepository CicMessageMailDocumentsRepository = new CicMessageMailDocumentsRepository();
                    foreach (MimePart attachment in msg.Attachments)
                    {
                        CicMessageMailDocuments CicMessageMailDocuments = new CicMessageMailDocuments();
                        CicMessageMailDocuments.CicMessageMailId = CicMessageMail.CicMessageMailId;
                        CicMessageMailDocuments.DateCreated = DateTime.Now;
                        CicMessageMailDocuments.DocumentName = attachment.ContentName;
                        CicMessageMailDocuments.DocumentType = attachment.ContentType.MimeType;
                        CicMessageMailDocuments.Document = attachment.BinaryContent;
                        CicMessageMailDocumentsRepository.InsertOrUpdate(CicMessageMailDocuments);
                        CicMessageMailDocumentsRepository.Save();
                    }

                }

            }

        }

    }
}