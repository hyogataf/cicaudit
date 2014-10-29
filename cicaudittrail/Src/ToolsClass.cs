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
            Debug.WriteLine("sqlToString = " + sqlToString);
            var check = sqlToString.Any(v => forbiddenSql.Contains(v));
            return check;
        }


        public static string GetResourceValueForVariable(string value)
        {
            var key = string.Format("{0}", value);

            return cicaudittrail.Resources.Properties.ResourceManager.GetString(key) ?? value.ToString();
        }

    }
}