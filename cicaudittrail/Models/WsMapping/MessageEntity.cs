using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cicaudittrail.Models.WsMapping
{
    public class MessagesEntity
    {

        public string ObjetMessage { get; set; }

        public string From { get; set; } // envoyé par

        public DateTime DateMessage { get; set; }

        public string Message { get; set; }

        public List<MessageEntityPJ> Attachements { get; set; }

    }


    public class MessageEntityPJ
    {
        public string file { get; set; } // Base64 encode

        public string fileName { get; set; }

        public string fileType { get; set; }

    }
}