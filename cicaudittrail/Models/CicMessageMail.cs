using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicMessageMail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICMESSAGEMAILID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicMessageMailId { get; set; }

         
        [Column("CICREQUESTRESULTSFOLLOWEDID")]
        public long CicRequestResultsFollowedId { get; set; }
        [Display(Name = "CicMessageMail_CicRequestResultsFollowed", ResourceType = typeof(Properties))]
        public virtual CicRequestResultsFollowed CicRequestResultsFollowed { get; set; }

        [Column("MESSAGESENT")]
        [Display(Name = "CicMessageMail_MessageSent", ResourceType = typeof(Properties))]
        public string MessageSent { get; set; }


        [Column("DATEMESSAGESENT")]
        [Display(Name = "CicMessageMail_DateMessageSent", ResourceType = typeof(Properties))]
        public DateTime? DateMessageSent { get; set; }


        [Column("USERMESSAGESENT")]
        [Display(Name = "CicMessageMail_UserMessageSent", ResourceType = typeof(Properties))]
        public string UserMessageSent { get; set; }

        [Column("MESSAGERECEIVED")]
        [Display(Name = "CicMessageMail_MessageReceived", ResourceType = typeof(Properties))]
        public string MessageReceived { get; set; }


        [Column("DATEMESSAGERECEIVED")]
        [Display(Name = "CicMessageMail_DateMessageReceived", ResourceType = typeof(Properties))]
        public DateTime? DateMessageReceived { get; set; }


        [Column("USERRESPONSESENT")]
        [Display(Name = "CicMessageMail_UserResponseSent", ResourceType = typeof(Properties))]
        public string UserResponseSent { get; set; }

    }
}