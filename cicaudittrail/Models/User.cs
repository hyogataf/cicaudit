using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;

namespace cicaudittrail.Models
{
    [DisplayColumn("Matricule")] //Fonction toString
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("USERID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Properties), ErrorMessageResourceName = "UserMatriculeRequired")] // champ obligatoire
        // texte affiché en cas d'erreur ("UserMatriculeRequired", géré par le fichier Properties.resx). La syntaxe, par convention, =TablePropriete
        [Display(Name = "UserMatricule", ResourceType = typeof(Properties))] //label affiché [géré par le fichier Properties.resx]
        [Column("MATRICULE")]
        public String Matricule { get; set; }

        [Display(Name = "UserNom", ResourceType = typeof(Properties))] //label affiché [géré par le Properties.resx]
        [Column("NOM")]
        public string Nom { get; set; } // ? == champ pas obligatoire

        [Display(Name = "UserPrenom", ResourceType = typeof(Properties))] //label affiché [géré par le Properties.resx]
        [Column("PRENOM")]
        public string Prenom { get; set; }
    }
}