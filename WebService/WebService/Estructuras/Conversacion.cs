using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Conversacion
    {
        public int conv_id { get; set; }

        public int cl_id { get; set; }

        public int admin_id { get; set; }

        public int conv_spam { set; get; }

        public int conv_estado { get; set; }

       
    }
}