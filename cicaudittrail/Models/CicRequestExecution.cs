using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicRequestExecution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICREQUESTEXECUTIONID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestExecutionId { get; set; }

        [Display(Name = "CicRequestExecutionCicRequestId", ResourceType = typeof(Properties))]
        [Column("CICREQUESTID")] 
        public long CicRequestId { get; set; }
        [Display(Name = "CicRequestExecutionCicRequest", ResourceType = typeof(Properties))]
        public virtual CicRequest CicRequest { get; set; } 
         
        [Display(Name = "CicRequestExecutionUserUpdated", ResourceType = typeof(Properties))] 
        [Column("CICREQUESTUSERUPDATED")]
        public string CicRequestUserUpdated { get; set; }
         
        [Display(Name = "CicRequestExecutionDateUpdated", ResourceType = typeof(Properties))]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Column("CICREQUESTDATEUPDATED")]
        public DateTime? CicRequestDateUpdated { get; set; }

        [Display(Name = "CicRequestExecutionUserExecuted", ResourceType = typeof(Properties))] 
        [Column("CICREQUESTUSEREXECUTED")]
        public string CicRequestUserExecuted { get; set; }

        [Display(Name = "CicRequestExecutionDateExecuted", ResourceType = typeof(Properties))]
        [Column("CICREQUESTDATEEXECUTED")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CicRequestDateExecuted { get; set; }

        [Display(Name = "CicRequestExecutionUserDeleted", ResourceType = typeof(Properties))]
        [Column("CICREQUESTUSERDELETED")]
        public string CicRequestUserDeleted { get; set; }

        [Display(Name = "CicRequestExecutionDateDeleted", ResourceType = typeof(Properties))]
        [Column("CICREQUESTDATEDELETED")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CicRequestDateDeleted { get; set; }


        [Display(Name = "CicRequestExecutionUserFollowed", ResourceType = typeof(Properties))]
        [Column("CICREQUESTRESULTSUSERFOLLOWED")]
        public string CicRequestResultsUserFollowed { get; set; }

        [Display(Name = "CicRequestExecutionDateFollowed", ResourceType = typeof(Properties))]
        [Column("CICREQUESTRESULTSDATEFOLLOWED")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CicRequestResultsDateFollowed { get; set; }

        [Display(Name = "CicRequestExecutionCicRequestResultsFollowedId", ResourceType = typeof(Properties))]
        [Column("CICREQUESTRESULTSFOLLOWEDID")]
        public long? CicRequestResultsFollowedId { get; set; }
        [Display(Name = "CicRequestExecutionCicRequestResultsFollowed", ResourceType = typeof(Properties))]
        public virtual CicRequestResultsFollowed CicRequestResultsFollowed { get; set; }

        [Column("DATECREATED")]
        public DateTime? DateCreated { get; set; }

    }
}