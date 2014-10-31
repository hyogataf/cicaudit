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



        [Display(Name = "CicRequestExecutionCicRequestResultsFollowedId", ResourceType = typeof(Properties))]
        [Column("CICREQUESTRESULTSFOLLOWEDID")]
        public long? CicRequestResultsFollowedId { get; set; }
        [Display(Name = "CicRequestExecutionCicRequestResultsFollowed", ResourceType = typeof(Properties))]
        public virtual CicRequestResultsFollowed CicRequestResultsFollowed { get; set; }



        [Column("CICMESSAGEMAILID")]
        public long? CicMessageMailId { get; set; }
        [Display(Name = "CicRequestExecution_CicMessageMail", ResourceType = typeof(Properties))]
        public virtual CicMessageMail CicMessageMail { get; set; }


        [Column("DATECREATED")]
        public DateTime? DateCreated { get; set; }


        [Display(Name = "CicRequestExecution_Action", ResourceType = typeof(Properties))]
        [Column("ACTION")]
        public string Action { get; set; }


        [Display(Name = "CicRequestExecution_UserAction", ResourceType = typeof(Properties))]
        [Column("USERACTION")]
        public string UserAction { get; set; }


        [Display(Name = "CicRequestExecution_DateAction", ResourceType = typeof(Properties))]
        [Column("DATEACTION")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateAction { get; set; }

    }

    public enum Action
    {
        U, E, D, F, FC, FA, MS
        //U: UPDATE, EXECUTION, D:DELETE, F:FOLLOW (suivi), FC: FOLLOW CONFIRMATION (suivi confirmé), FA: FOLLOW ABORTED (suivi annulé), MS: MAIL SENT (justificatis demandés)
    }
}