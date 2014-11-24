using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cicaudittrail.Models
{
    public class CicDiversRequestResults
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICDIVERSREQUESTRESULTSID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicDiversRequestResultsId { get; set; }

        [Column("CODE")]
        public String Code { get; set; }

        [Column("CRITERIA")]
        public string Criteria { get; set; }

        [Column("ROWCONTENT")]
        public string RowContent { get; set; }

        [Column("DATECREATED")]
        public DateTime DateCreated { get; set; }

    }
}