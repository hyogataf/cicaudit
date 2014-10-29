using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cicaudittrail.Resources;
using System.Web.Mvc;

namespace cicaudittrail.Models
{
    [DisplayColumn("Libelle")] //Fonction toString
    public class CicMessageTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICMESSAGETEMPLATEID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicMessageTemplateId { get; set; }

        [Column("CODE")]
        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicMessageTemplateCodeRequired")] // champ obligatoire
        [Display(Name = "CicMessageTemplateCode", ResourceType = typeof(Properties))]
        public string Code { get; set; }

        [Column("LIBELLE")]
        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicMessageTemplateLibelleRequired")]
        [Display(Name = "CicMessageTemplateLibelle", ResourceType = typeof(Properties))]
        public string Libelle { get; set; }

        [Column("OBJETMAIL")]
        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicMessageTemplateObjetMailRequired")]
        [Display(Name = "CicMessageTemplateObjetMail", ResourceType = typeof(Properties))]
        public string ObjetMail { get; set; }

        [Column("CONTENUMAIL")]
        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicMessageTemplateContenuMailRequired")]
        [Display(Name = "CicMessageTemplateContenuMail", ResourceType = typeof(Properties))]
        [DataType(DataType.MultilineText)] // longtext
        [AllowHtml]
        public string ContenuMail { get; set; }

        [Column("DATECREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicMessageTemplateDateCreated", ResourceType = typeof(Properties))]
        public DateTime DateCreated { get; set; }

        [Column("USERCREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicMessageTemplateUserCreated", ResourceType = typeof(Properties))]
        public string UserCreated { get; set; }

    }
}