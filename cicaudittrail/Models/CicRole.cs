using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    public class CicRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICROLEID")]
        public long CicRoleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "RoleNameRequired")] // champ obligatoire
        [Display(Name = "RoleName", ResourceType = typeof(Properties))] //label affiché [géré par le Properties.resx]
        [Column("NAME")]
        public String Name { get; set; }


        [Column("DATECREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicRole_DateCreated", ResourceType = typeof(Properties))]
        public DateTime DateCreated { get; set; }

        [Column("USERCREATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicRole_UserCreated", ResourceType = typeof(Properties))]
        public string UserCreated { get; set; }


        [Column("DATEUPDATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicRole_DateUpdated", ResourceType = typeof(Properties))]
        public DateTime DateUpdated { get; set; }

        [Column("USERUPDATED")]
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Display(Name = "CicRole_UserUpdated", ResourceType = typeof(Properties))]
        public string UserUpdated { get; set; }
    }
}