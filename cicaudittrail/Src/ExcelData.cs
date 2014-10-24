using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel;
using System.IO;
using System.Diagnostics;
using System.Data;

namespace cicaudittrail.Src
{
    public class ExcelData
    {
        string _path;

        public ExcelData(string path)
        {
            _path = path;
        }


        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);

            // We return the interface, so that 
            IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                return reader;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("getExcelReader.StackTrace= " + ex.StackTrace);
                throw;
            }
        }


        public IEnumerable<string> getWorksheetNames()
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }


        public IEnumerable<DataRow> getData(string sheet, bool firstRowIsColumnNames = true)
        {
            try
            {
                var reader = this.getExcelReader();
                reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
                var workSheet = reader.AsDataSet().Tables[sheet];
                Debug.WriteLine("workSheet ToString  = " + workSheet.ToString());
                Debug.WriteLine("workSheet Columns = " + workSheet.Columns);
                var rows = from DataRow a in workSheet.Rows select a;
                return rows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("getData error = "+ex.StackTrace);
                //throw;
                return null;
            }
        }


    }


    //Class utilisée afin de mapper le contenu du fichier excel uploadé lors du suivi manuel
    //C'est un fichier de 2 colonnes: id (id de la table CicRequestResults) et commentaires
    public class ExcelSuiviMapping
    {
        public string Id { get; set; }
        public string Comments { get; set; } 
    }



}