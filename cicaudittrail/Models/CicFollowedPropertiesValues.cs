using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicFollowedPropertiesValues
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues 
        [Column("CICFOLLOWEDPROPERTIESVALUESID")]
        public long CicFollowedPropertiesValuesId { get; set; }

        [Column("CICREQUESTRESULTSFOLLOWEDID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestResultsFollowedId { get; set; }
        [Display(Name = "CicFollowedPropertiesValues_CicRequestResultsFollowed", ResourceType = typeof(Properties))]
        public virtual CicRequestResultsFollowed CicRequestResultsFollowed { get; set; }

        [Column("PROPERTY")]
        [Display(Name = "CicFollowedPropertiesValues_Property", ResourceType = typeof(Properties))]
        public string Property { get; set; }

        [Column("VALUE")]
        [Display(Name = "CicFollowedPropertiesValues_Value", ResourceType = typeof(Properties))]
        public string Value { get; set; }


        [Column("DATECREATED")]
        [Display(Name = "CicFollowedPropertiesValues_DateCreated", ResourceType = typeof(Properties))]
        public DateTime? DateCreated { get; set; }

    }
}