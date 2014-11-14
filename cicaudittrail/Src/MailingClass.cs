using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
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
                string dwml = webClient.DownloadString(CommunicationWSUri);


            }

        }

    }
}