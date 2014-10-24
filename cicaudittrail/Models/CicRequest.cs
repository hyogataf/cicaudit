using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    [DisplayColumn("Libelle")] //Fonction toString
    public class CicRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICREQUESTID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicRequestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicRequestCodeRequired")] // champ obligatoire
        [Display(Name = "CicRequestCode", ResourceType = typeof(Properties))] //label affiché [géré par le fichier Properties.resx]
        [Column("CODE")]
        public String Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicRequestLibelleRequired")] // champ obligatoire
        [Display(Name = "CicRequestLibelle", ResourceType = typeof(Properties))] //label affiché [géré par le fichier Properties.resx]
        [Column("LIBELLE")]
        public String Libelle { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicRequestRequestRequired")] // champ obligatoire
        [Display(Name = "CicRequestRequest", ResourceType = typeof(Properties))] //label affiché [géré par le fichier Properties.resx]
        [Column("REQUEST")]
        [DataType(DataType.MultilineText)] // longtext
        public String Request { get; set; }

        public virtual ICollection<CicRequestExecution> CicRequestExecution { get; set; }

        public virtual ICollection<CicRequestResultsFollowed> CicRequestResultsFollowed { get; set; }
         
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("ISDELETED")]
        //[NotMapped]
        public int IsDeleted { get; set; }
    }
}