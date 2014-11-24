using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Common;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace cicaudittrail.Src
{
    public class ToolsClass
    {
        static HashSet<string> forbiddenSql = new HashSet<string> { "associate", "disassociate", "update", "insert", "set", "drop", "create", "alter", "commit", "connect", "constraint", "delete", "exec", "execute", "expdp", "explain", "grant", "impdp", "lock", "materialized", "merge", "noaudit", "purge", "recover", "rename", "revoke", "rman", "roolback", "shutdown", "savepoint", "snapshot", "subquery", "startup", "tnsping", "truncate" };


        public bool CheckSql(string sqlRequest)
        {
            List<string> sqlToString = sqlRequest.Split(' ').ToList();
            // Debug.WriteLine("sqlToString = " + sqlToString);
            var check = sqlToString.Any(v => forbiddenSql.Contains(v));
            return check;
        }


        public static string GetResourceValueForVariable(string value)
        {
            var key = string.Format("{0}", value);

            return cicaudittrail.Resources.Properties.ResourceManager.GetString(key) ?? value.ToString();
        }

        /*
         * Methode utilisée pour generer les contenus des messages.
         * Reçoit un contenu en template (avec des variables), et un map contenant des paires; chaque paire etant une variable du template et sa valeur.
         * La méthode boucle sur le map et remplace les variables par leur valeur.
         * */
        public string generateBodyMessage(Dictionary<string, string> mapValues, string BodyMessageTemplate)
        {
            try
            {
                string msg = BodyMessageTemplate;
                foreach (KeyValuePair<string, string> element in mapValues)
                {
                    msg = msg.Replace(element.Key, element.Value);
                }
                return msg;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToolsClass generateBodyMessage error = " + ex.StackTrace);
                return BodyMessageTemplate;
            }
        }



        // decode from base64 des string
        public byte[] Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }


        //encodage en base64 des byte[]
        public string Base64Encode(byte[] byteElement)
        {
            return System.Convert.ToBase64String(byteElement);
        }



        //methode qui recupere des resultats d'une requete sql et les range sous forme de json. Chaque enregistrements des resultats trouvés devient une ligne d'une liste de json
        //methode reprise dans RequestExecutionJob, en cas de modif
        public static List<string> ReadToJSON(DbDataReader reader)
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

    }
}