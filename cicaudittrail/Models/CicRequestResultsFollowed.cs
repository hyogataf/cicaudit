using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicRequestResultsFollowed
    {
        [Column("CICREQUESTRESULTSFOLLOWEDID")]
        public long CicRequestResultsFollowedId { get; set; }

        [Column("CICREQUESTID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestId { get; set; }
        [Display(Name = "CicRequestResultsFollowedCicRequest", ResourceType = typeof(Properties))]
        public virtual CicRequest CicRequest { get; set; }

        [Column("ROWCONTENT")]
        [Display(Name = "CicRequestResultsFollowedRowContent", ResourceType = typeof(Properties))]
        public string RowContent { get; set; }

        [Column("DATECREATED")]
        [Display(Name = "CicRequestResultsFollowedDateCreated", ResourceType = typeof(Properties))]
        public DateTime DateCreated { get; set; }

        [Column("USERCREATED")]
        [Display(Name = "CicRequestResultsFollowedUserCreated", ResourceType = typeof(Properties))]
        public string UserCreated { get; set; }

        [Column("STATUT")]
        [Display(Name = "CicRequestResultsFollowedStatut", ResourceType = typeof(Properties))]
        //public Statut? Statut { get; set; } // Ne sait pas binder sous Oracle. So, utilise un string en attendant
        public string Statut { get; set; }

        [Column("COMMENTS")]
        [Display(Name = "CicRequestResultsFollowedComments", ResourceType = typeof(Properties))]
        public string Comments { get; set; }

    }

    public enum Statut
    {
        E, A, ME, MR, S, C
        //En attente, Annulé, Suspect, Centif tiré, Mail envoyé au gestionnaire, Mail repondu (par le gestionnaire)
    }
}