using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Diagnostics;
using cicaudittrail.Models;
using System.Data.Common;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace cicaudittrail.Jobs
{
    public class RequestExecutionJob : IJob
    {


        public void Execute(IJobExecutionContext context)
        {
            /*  try
              {
                  ICicRequestRepository CicrequestRepository = new CicRequestRepository();
                  Debug.WriteLine("job executed at= " + DateTime.Now);

                  var listRequests = CicrequestRepository.All;
                  foreach (var requestInstance in listRequests)
                  {
                      if (string.IsNullOrEmpty(requestInstance.Request) == false)
                      {
                          using (var ctx = CicrequestRepository.GetContext)
                          using (var cmd = ctx.Database.Connection.CreateCommand())
                          {
                              //on supprime d'abord les précédents résultats de la requete
                              truncateTable(requestInstance.CicRequestId);

                              //on ouvre une chaine de connexion avec le contexte
                              ctx.Database.Connection.Open();
                              cmd.CommandText = requestInstance.Request;
                              using (var reader = cmd.ExecuteReader())
                              {
                                  //on récupere les resultats de la requete, puis on la garde dans une liste
                                  var model = Read(reader).ToList();

                                  //on boucle sur la liste, et chaque element de la liste est enregistrée dans la table CicRequestResults
                                  foreach (var line in model)
                                  {
                                      //chaque element de la liste (qui est une ligne de l'ensemble des resultats de la requete executée) est transformée en string, avec ',' comme séparateur, puis stockée dans la table CicRequestResults
                                      var rowContent = String.Join("|#|", line.ToArray());

                                      //Insert command
                                      string insertsql = "insert into CicRequestResults(CicRequestId, RowContent, DateCreated) values(:P0,:P1,:P2)";
                                      List<object> parameterList = new List<object>();
                                      parameterList.Add(requestInstance.CicRequestId);
                                      parameterList.Add(rowContent);
                                      parameterList.Add(DateTime.Now);
                                      object[] parameters = parameterList.ToArray();

                                      int result = ctx.Database.ExecuteSqlCommand(insertsql, parameters); 
                                  }

                              }
                          }
                      }
                  }
              }
              catch (Exception ex)
              {
                  Debug.WriteLine("Execute StackTrace = " + ex.StackTrace);
              }*/

            try
            {
                ICicRequestRepository CicrequestRepository = new CicRequestRepository();
                Debug.WriteLine("job executed at= " + DateTime.Now);

                var listRequests = CicrequestRepository.All;
                foreach (var requestInstance in listRequests)
                {
                    if (string.IsNullOrEmpty(requestInstance.Request) == false)
                    {
                        using (var ctx = CicrequestRepository.GetContext)
                        using (var cmd = ctx.Database.Connection.CreateCommand())
                        {
                            //on supprime d'abord les précédents résultats de la requete
                           var del = truncateTable(requestInstance.CicRequestId);
                           Debug.WriteLine("truncate result  = " + del);

                            //on ouvre une chaine de connexion avec le contexte
                            ctx.Database.Connection.Open();
                            cmd.CommandText = requestInstance.Request;
                            using (var reader = cmd.ExecuteReader())
                            {
                                //on récupere les resultats de la requete, puis on la garde dans une liste
                                var model = ReadToJSON(reader).ToList();
                                Debug.WriteLine("model result  = " + model.Count);
                                //Debug.WriteLine("model result 1 = " + model[0]);
                                //on boucle sur la liste, et chaque element de la liste est enregistrée dans la table CicRequestResults
                                foreach (var line in model)
                                {
                                    //chaque element de la liste (qui est une ligne de l'ensemble des resultats de la requete executée) est transformée en string, avec ',' comme séparateur, puis stockée dans la table CicRequestResults 
                                    //Debug.WriteLine("actual line = " + line);

                                    //Insert command
                                    string insertsql = "insert into CicRequestResults(CicRequestId, RowContent, DateCreated) values(:P0,:P1,:P2)";
                                    List<object> parameterList = new List<object>();
                                    parameterList.Add(requestInstance.CicRequestId);
                                    parameterList.Add(line);
                                    parameterList.Add(DateTime.Now);
                                    object[] parameters = parameterList.ToArray();

                                    int result = ctx.Database.ExecuteSqlCommand(insertsql, parameters);

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execute StackTrace = " + ex.StackTrace);
            }
        }

        //methode qui recupere des resultats d'une requete sql et les range sous forme de list. Chaque enregistrements des resultats trouvés devient une ligne de la list
        private static List<object[]> Read(DbDataReader reader)
        {
            var count = 0;
            List<object[]> concateneList = new List<object[]>();
            while (reader.Read())
            {
                var values = new List<object>();
                if (count == 0)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader.GetName(i));
                    }
                }
                else
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader.GetValue(i));
                    }
                }
                count++;
                concateneList.Add(values.ToArray());
                //yield return values.ToArray();
            }
            return concateneList;
        }



        //methode qui recupere des resultats d'une requete sql et les range sous forme de json. Chaque enregistrements des resultats trouvés devient une ligne d'une liste de json
        private static List<string> ReadToJSON(DbDataReader reader)
        {
            List<string> concateneList = new List<string>();

            while (reader.Read())
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                //var values = new List<object>();
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        writer.WritePropertyName(reader.GetName(i));
                        //Debug.WriteLine("reader.GetValue(i)= " + reader.GetValue(i));
                        writer.WriteValue(reader.GetValue(i));
                    }
                    writer.WriteEnd();
                    writer.Close();
                    sw.Close();
                    //writer.WriteEndObject();
                }
                concateneList.Add(sb.ToString());
            }
            return concateneList;
        }

        // methode de suppression de la table CicRequestResults, selon le champ CicRequestId
        private int truncateTable(long CicRequestId)
        {
            try
            {
                ICicRequestRepository CicrequestRepository = new CicRequestRepository();
                using (var ctx = CicrequestRepository.GetContext)
                {
                    string deletesql = "delete from CicRequestResults where CicRequestId=:P0";
                    List<object> parameterList = new List<object>();
                    parameterList.Add(CicRequestId);
                    object[] parameters = parameterList.ToArray();

                    int result = ctx.Database.ExecuteSqlCommand(deletesql, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("truncateTable StackTrace = " + ex.StackTrace);
                return 0;
            }
        }
    }
}