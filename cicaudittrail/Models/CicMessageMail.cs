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
        public long? CicRequestResultsFollowedId { get; set; }
        [Display(Name = "CicMessageMail_CicRequestResultsFollowed", ResourceType = typeof(Properties))]
        public virtual CicRequestResultsFollowed CicRequestResultsFollowed { get; set; }

        [Column("MESSAGECONTENT")]
        [Display(Name = "CicMessageMail_MessageContent", ResourceType = typeof(Properties))]
        public string MessageContent { get; set; }

        [Column("OBJETMESSAGE")]
        [Display(Name = "CicMessageMail_ObjetMessage", ResourceType = typeof(Properties))]
        public string ObjetMessage { get; set; }

        [Column("DATEMESSAGE")]
        [Display(Name = "CicMessageMail_DateMessage", ResourceType = typeof(Properties))]
        public DateTime? DateMessage { get; set; }


        [Column("USERMESSAGE")]
        [Display(Name = "CicMessageMail_UserMessage", ResourceType = typeof(Properties))]
        public string UserMessage { get; set; }

        [Column("SENS")]
        [Display(Name = "CicMessageMail_Sens", ResourceType = typeof(Properties))]
        public string Sens { get; set; }

        [Display(Name = "CicMessageMail_CicMessageMailDocuments", ResourceType = typeof(Properties))]
        public virtual ICollection<CicMessageMailDocuments> CicMessageMailDocuments { get; set; } // hasMany. Ne pas oublier de modifier le context
    }

     public enum Sens
    {
        I, O
        //I: In (message reçu), O: Out (message envoyé)
    }
} 