using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EtzJaimWebPage.Models
{
    public class Conversacion_User
    {
        public int id { get; set; }

        public int conv_id { get; set; }

        public string username { get; set; }

        public int conv_spam { get; set; }

        public int conv_estado { get; set; }
    }
}