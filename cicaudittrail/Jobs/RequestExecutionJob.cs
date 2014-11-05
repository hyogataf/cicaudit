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
using cicaudittrail.Src;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Threading.Tasks;

namespace cicaudittrail.Jobs
{
    public class RequestExecutionJob : IJob
    {
        static int InternalPadRowNumber = 50;



        public void OldExecuteMethodThatWorksButMonothread(IJobExecutionContext context)
        {
            try
            {
                Debug.WriteLine("job executed at= " + DateTime.Now);
                ICicRequestRepository CicrequestRepository = new CicRequestRepository();

                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cicaudittrailContext"].ConnectionString);
                var cmd = conn.CreateCommand();
                conn.Open();


                var listRequests = CicrequestRepository.All;
                //   var ctx = CicrequestRepository.GetContext;
                //   var cmd = ctx.Database.Connection.CreateCommand();

                //on ouvre une chaine de connexion avec le contexte
                //   ctx.Database.Connection.Open();

                foreach (var requestInstance in listRequests)
                {
                    ToolsClass tools = new ToolsClass();
                    if (string.IsNullOrEmpty(requestInstance.Request) == false && tools.CheckSql(requestInstance.Request) == false)
                    {

                        //  using (var ctx = CicrequestRepository.GetContext)
                        /*    using (var cmd = ctx.Database.Connection.CreateCommand())
                            {*/

                        //on supprime d'abord les précédents résultats de la requete
                        //   var del = truncateTable(requestInstance.CicRequestId);
                        //  Debug.WriteLine("truncate result  = " + del);
                        var sqlrequest = PaginateSql(requestInstance.Request);

                        cmd.CommandText = sqlrequest;
                        var executionNumber = 0;
                        do
                        {
                            Debug.WriteLine(" executionNumber = " + executionNumber);
                            executionNumber = executeSql(executionNumber, cmd, requestInstance);
                            OracleConnection.ClearAllPools();
                        } while (executionNumber > 0);


                        /*using (var reader = cmd.ExecuteReader())
                        {
                            //   var reader = cmd.ExecuteReader();
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

                                int result = CicrequestRepository.GetContext.Database.ExecuteSqlCommand(insertsql, parameters);
                                // int result = conn.Database.ExecuteSqlCommand(insertsql, parameters);


                            }
                            Debug.WriteLine("before closing = ");
                            //reader.Close();


                        } */
                        //end using (var reader = cmd.ExecuteReader())


                        //}//end using (var cmd = ctx.Database.Connection.CreateCommand())
                    }
                }// end foreach (var requestInstance in listRequests)
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execute StackTrace = " + ex.StackTrace);
            }
        }



        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Debug.WriteLine("job executed at= " + DateTime.Now);
                ICicRequestRepository CicrequestRepository = new CicRequestRepository();




                var listRequests = CicrequestRepository.All;
                /*   OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cicaudittrailContext"].ConnectionString);
                   var cmd = conn.CreateCommand();
                   conn.Open();
                   */

                //TODO controller le nombre de tasks créés. 
                /*
                 * Ne plus avoir 'autant de tasks que de cicRequests'. 
                 * Ex: 2 cicRequests par tasks
                 * int nbreRequests = 2;
                 * var index = 0;
                 * foreach (var requestInstance in listRequests){
                 *      ToolsClass tools = new ToolsClass();
                        if (string.IsNullOrEmpty(requestInstance.Request) == false && tools.CheckSql(requestInstance.Request) == false)
                        {     
                 *          if (index % 2 == 0) {
                 *              Task.Factory.StartNew
                 *              .
                 *              .
                 *              .
                 *             
                 *          }
                 *           index++;
                 * }
                 * */
                foreach (var requestInstance in listRequests)
                {
                    ToolsClass tools = new ToolsClass();
                    if (string.IsNullOrEmpty(requestInstance.Request) == false && tools.CheckSql(requestInstance.Request) == false)
                    {
                        CicRequest requestCopy = requestInstance;
                        Debug.WriteLine("Before ExecuteTrigger = " + requestCopy.CicRequestId);
                        //On declenche une tache d'execution de l'action en parallele
                        Task.Factory.StartNew(() => { ExecuteTrigger(requestCopy); });

                        /*    var sqlrequest = PaginateSql(requestInstance.Request);

                            cmd.CommandText = sqlrequest;
                            var executionNumber = 0;
                            do
                            {
                                Debug.WriteLine(" executionNumber = " + executionNumber);
                                executionNumber = executeSql(executionNumber, cmd, requestInstance);
                                OracleConnection.ClearAllPools();
                            } while (executionNumber > 0);*/

                    }
                }// end foreach (var requestInstance in listRequests)
                /*  cmd.Dispose();
                  conn.Close();*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execute StackTrace = " + ex.StackTrace);
            }
        }


        private static void ExecuteTrigger(CicRequest requestInstance)
        {
            Debug.WriteLine("ExecuteTrigger = " + requestInstance.CicRequestId + " ; at = " + DateTime.Now);
            var sqlrequest = PaginateSql(requestInstance.Request);
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cicaudittrailContext"].ConnectionString);
            var cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = sqlrequest;
            var executionNumber = 0;
            do
            {
                Debug.WriteLine(" executionNumber = " + requestInstance.CicRequestId+" // " + executionNumber);
                executionNumber = executeSql(executionNumber, cmd, requestInstance);
                OracleConnection.ClearAllPools();
            } while (executionNumber > 0);

            cmd.Dispose();
            conn.Close();
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
                    /* i est initialisé à 1 du fait de PaginateSql.
                     * Cette methode ajoute un champ au select, necessaire pour le programme, mais qui ne devrait pas s'afficher pour l'utilisateur.
                     * i demarrant à 1 permet de sauter ce champ
                     * */
                    for (int i = 1; i < reader.FieldCount; i++)
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
        private static int truncateTable(long CicRequestId)
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


        /*
         * Methode de pagination de la requete.
         * Elle reçoit une requete à executer et lui adjoint du sql permettant de paginer.
         * 1. Elle lui prepend: << SELECT t.* FROM ( SELECT ROWNUM AS rn, t.* FROM ( >>
         * 2. Et lui append: << ) t  WHERE ROWNUM <= 30  ) t WHERE rn >= 10 >>
         * 30 et 10 sont des parametres 
         * */
        private static string PaginateSql(string sqlRequest)
        {
            try
            {
                //On verifie que la requete ne se termine pas par ';'
                var sql = sqlRequest.Trim();
                if (sql.EndsWith(";"))
                {
                    sql = sql.Substring(0, sql.Length - 1);
                }

                //prepending
                var prepend = "SELECT t.* FROM ( SELECT ROWNUM AS rn, t.* FROM ( ";
                sql = prepend + sql;
                //   Debug.WriteLine("sql prepend = " + sql);

                //appending
                var append = " ) t  WHERE ROWNUM <= :P_LAST_ROW  ) t WHERE rn >= :P_FIRST_ROW";
                sql = sql + append;
                //   Debug.WriteLine("sql append = " + sql);
                return sql;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("truncateTable StackTrace = " + ex.StackTrace);
                return sqlRequest;
            }
        }


        private static int executeSql(int startRowNumber, OracleCommand cmd, CicRequest requestInstance)
        {
            try
            {
                ICicRequestRepository CicrequestRepository = new CicRequestRepository();
                var ExternalPadRowNumber = ConfigurationManager.AppSettings["PadRowNumber"];
                var PadRowNumber = 0;
                int n;
                bool isNumeric = int.TryParse(ExternalPadRowNumber, out n);
                if (isNumeric == false || ExternalPadRowNumber == null)
                {
                    PadRowNumber = InternalPadRowNumber;
                }
                else
                {
                    var IntExternalPadRowNumber = Int32.Parse(ExternalPadRowNumber);
                    if (IntExternalPadRowNumber == 0)
                    {
                        PadRowNumber = InternalPadRowNumber;
                    }
                    else
                    {
                        PadRowNumber = IntExternalPadRowNumber;
                    }
                }

                int endRowNumber = PadRowNumber + startRowNumber;

                Debug.WriteLine(" P_FIRST_ROW = " + startRowNumber);
                Debug.WriteLine(" P_LAST_ROW = " + endRowNumber);

                cmd.Parameters.Add("P_LAST_ROW", endRowNumber);
                cmd.Parameters.Add("P_FIRST_ROW", startRowNumber);
                using (var reader = cmd.ExecuteReader())
                {
                    Debug.WriteLine(" reader = " + reader.HasRows + " , at " + DateTime.Now);

                    if (reader.HasRows)
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

                            int result = CicrequestRepository.GetContext.Database.ExecuteSqlCommand(insertsql, parameters);
                            // int result = conn.Database.ExecuteSqlCommand(insertsql, parameters); 
                        }
                        Debug.WriteLine("before closing = ");
                        //reader.Close();
                        endRowNumber++;
                    }
                    else
                    {
                        endRowNumber = 0;
                    }
                    cmd.Parameters.Clear();

                    return endRowNumber;
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
 