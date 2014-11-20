using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace cicaudittrail.Src
{
    public static class Extensions
    {
        public static string ToCSV(this DataTable table, string delimator)
        {
            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
            }
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    // Debug.WriteLine("column = " + row[i].ToString());
                    var content = row[i].ToString().Replace("\"", "\"\"");
                    if (content.IndexOf(',') >= 0)
                        content = '"' + content + '"';
                    //   Debug.WriteLine("column = " + row[i].ToString());
                    result.Append(content);
                    result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
            //return result.ToString();
        }
    }
}