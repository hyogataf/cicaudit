using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    [DisplayColumn("Name")] //Fonction toString
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("ROLEID")]
        public long RoleId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "RoleNameRequired")] // champ obligatoire
        [Display(Name = "RoleName", ResourceType = typeof(Properties))] //label affiché [géré par le Properties.resx]
        [Column("NAME")]
        public String Name { get; set; }
    }
}