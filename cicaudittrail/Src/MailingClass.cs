using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using cicaudittrail.Models;
using System.Collections;
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
        public long SendEmail(ArrayList paramsList)
        {
            using (var client = new HttpClient())
            {
                //    client.BaseAddress = new Uri();
                var uri = ConfigurationManager.AppSettings["SendMailSmtp"] + ConfigurationManager.AppSettings["Appname"];
                client.BaseAddress = new Uri(uri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                long SMTPProviderMessageId = 0;
                Task task = Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("inside task ");
                HttpResponseMessage response = client.PostAsJsonAsync("sendmail/" + ConfigurationManager.AppSettings["Appname"], paramsList).Result;
                Debug.WriteLine("inside task response = " + response);
                Debug.WriteLine("inside task response StatusCode = " + response.StatusCode);
                Debug.WriteLine("inside task response IsSuccessStatusCode= " + response.Content.ToString());
                //On recupere l'id du message renvoyé par le provider dans response.Content.ToString() 
                int ignoreMe;
                bool successfullyParsed = int.TryParse(response.Content.ToString(), out ignoreMe);
                if (successfullyParsed)
                {
                    SMTPProviderMessageId = Convert.ToInt64(response.Content.ToString());
                }
            });
                task.Wait();
                return SMTPProviderMessageId;
            }




            /* using (MailMessage mailMessage = new MailMessage())
             {
                 mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                 mailMessage.Subject = subject;
                 mailMessage.Body = body;
                 mailMessage.IsBodyHtml = true;
                 mailMessage.To.Add(new MailAddress(recepientEmail));
                 //ajout des pj
                 MemoryStream stream = new MemoryStream();
                 foreach (var pj in pjs)
                 {
                     stream = new MemoryStream(pj.Key);
                     Attachment attachment = new Attachment(stream, pj.Value);
                     mailMessage.Attachments.Add(attachment);
                 }


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

                 stream.Dispose();
             }*/
        }


    }
}