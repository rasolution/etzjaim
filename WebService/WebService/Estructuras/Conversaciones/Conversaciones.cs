using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras.Conversaciones
{
    public class Conversaciones
    {
        public int conv_id { get; set; }

        public string username1 { get; set; }

        public string username2 { get; set; }

        public int user1_spam { get; set; }

        public int user1_estado { get; set; }

        public int user2_spam { get; set; }

        public int user2_estado { get; set; }
    }
}