using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cicaudittrail.Resources;
using cicaudittrail.Models.WsMapping;
using cicaudittrail.Src;

namespace cicaudittrail.Models
{
    public class CicMessageMailDocuments
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [ScaffoldColumn(false)] // champ pas affiché dans les vues
        [Column("CICMESSAGEMAILDOCUMENTSID")] // Syntaxe du champ utilisé lors des requetes SQL. Obligatoire dans le cas d'une base Oracle
        public long CicMessageMailDocumentsId { get; set; }

        [Column("CICMESSAGEMAILID")]
        public long? CicMessageMailId { get; set; }
        [Display(Name = "CicMessageMailDocuments_CicMessageMail", ResourceType = typeof(Properties))]
        public virtual CicMessageMail CicMessageMail { get; set; }

        [Column("DOCUMENTNAME")]
        [Display(Name = "CicMessageMailDocuments_DocumentName", ResourceType = typeof(Properties))]
        public string DocumentName { get; set; }

        [Column("DOCUMENTTYPE")]
        [Display(Name = "CicMessageMail_DocumentType", ResourceType = typeof(Properties))]
        public string DocumentType { get; set; }

        [Column("DOCUMENT")]
        [Display(Name = "CicMessageMailDocuments_DocumentType", ResourceType = typeof(Properties))]
        public byte[] Document { get; set; }

        [Column("DATECREATED")]
        [Display(Name = "CicMessageMailDocuments_DateCreated", ResourceType = typeof(Properties))]
        public DateTime DateCreated { get; set; }


        public MessageEntityPJ ConvertToMessageEntityPJ()
        {
            ToolsClass tools = new ToolsClass();
            MessageEntityPJ entity = new MessageEntityPJ();
            entity.file = tools.Base64Encode(this.Document);
            entity.fileName = this.DocumentName;
            entity.fileType = DocumentType;
            return entity;
        }
    }
}