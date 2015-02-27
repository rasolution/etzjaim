using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Conversaciones_Cliente_Admin
    {
        public int conv_id { get; set; }

        public int cl_id { get; set; }

        public int admin_id { get; set; }

        public int conv_spam { set; get; }

        public int conv_estado { get; set; }

        public string cl_username { set; get; }

        public string admin_username { set; get; }
    }
}