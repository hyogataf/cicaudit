using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace cicaudittrail.Models
{
    public class CicRequestResults
    {
        [Column("CICREQUESTRESULTSID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestResultsId { get; set; }

        [Column("CICREQUESTID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestId { get; set; }

        [Column("ROWCONTENT")]
        public string RowContent { get; set; }
         
        [Column("DATECREATED")]
        public DateTime DateCreated { get; set; }
    }
}