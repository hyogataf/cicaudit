using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using cicaudittrail.Models;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;
//using OpenPop.Pop3;

namespace cicaudittrail.Src
{
    public class MailingClass
    {
        /*
         * Methode d'envoi de mails.
         * Les credentials de l'adresse mail sont stockés au niveau du fichier Web.config
         * */
        public bool SendEmail(ArrayList paramsList)
        {
            using (var client = new HttpClient())
            {
                //    client.BaseAddress = new Uri();
                var uri = ConfigurationManager.AppSettings["SendMailSmtp"]/* + ConfigurationManager.AppSettings["Appname"]*/;
                client.BaseAddress = new Uri(uri);

                //  client.DefaultRequestHeaders.Accept.Clear();
                //   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // long SMTPProviderMessageId = 0;
                bool etat = false;
                Task task = Task.Factory.StartNew(() =>
            {
                //  Debug.WriteLine("inside task ");
                var tailUri = "sendmail/" + ConfigurationManager.AppSettings["Appname"];
                //   Debug.WriteLine("inside task tailUri = " + tailUri);

                HttpResponseMessage response = client.PostAsJsonAsync(tailUri, paramsList).Result;

                var objectTask = response.Content.ReadAsAsync<bool>().ContinueWith(u =>
                {
                    var myobject = u.Result;
                    Debug.WriteLine("myobject = " + myobject);

                    bool ignoreMe;

                    bool successfullyParsed = Boolean.TryParse(myobject.ToString(), out ignoreMe);
                    Debug.WriteLine("successfullyParsed = " + successfullyParsed);

                    if (successfullyParsed)
                    {
                        etat = Convert.ToBoolean(myobject.ToString());
                    }
                    //do stuff 
                });
                objectTask.Wait();
                //    Debug.WriteLine("inside task response = " + response);
                // Debug.WriteLine("inside task response StatusCode = " + response.StatusCode);
                //  Debug.WriteLine("inside task response IsSuccessStatusCode= " + response.Content.ToString());
                //On recupere l'id du message renvoyé par le provider dans response.Content.ToString() 

            });
                task.Wait();
                return etat;
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