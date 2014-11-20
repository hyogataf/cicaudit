using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Src;

namespace cicaudittrail.Models.CustomValidation
{
    // class qui interdit l'insertion d'une liste de mot
    public class CheckSqlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid
   (object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string)value) == false)
            {
                string request = value.ToString();
                string sErrorMessage = "Votre requete contient des elements interdits, principalement des elements qui modifient la base de données.";
                ToolsClass tools = new ToolsClass();
                var check = tools.CheckSql(request);
                if (check == true)
                {
                    return new ValidationResult(sErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}