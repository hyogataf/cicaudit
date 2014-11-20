using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

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


    }
}