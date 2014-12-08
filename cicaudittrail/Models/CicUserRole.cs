using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicUserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICUSERROLEID")]
        public long CicUserRoleId { get; set; }


        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicUserRole_CicRoleId_Required")] // champ obligatoire 
        [Column("CICROLEID")]
        public long CicRoleId { get; set; }
        [Display(Name = "CicUserRole_Role", ResourceType = typeof(Properties))]
        public virtual CicRole CicRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "CicUserRole_Username_Required")] // champ obligatoire
        [Display(Name = "CicUserRole_Username", ResourceType = typeof(Properties))] //label affiché [géré par le Properties.resx]
        [DataType(DataType.EmailAddress, ErrorMessage = "Veuillez saisir l'adresse email de l'agent")]
        [Column("USERNAME")]
        public String Username { get; set; }

        [Column("DATECREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicUserRole_DateCreated", ResourceType = typeof(Properties))]
        public DateTime DateCreated { get; set; }

        [Column("USERCREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicUserRole_UserCreated", ResourceType = typeof(Properties))]
        public string UserCreated { get; set; }

        [Column("DATEUPDATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicUserRole_DateUpdated", ResourceType = typeof(Properties))]
        public DateTime DateUpdated { get; set; }

        [Column("USERUPDATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicUserRole_UserUpdated", ResourceType = typeof(Properties))]
        public string UserUpdated { get; set; }
    }
}