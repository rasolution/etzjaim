using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras.Conversaciones
{
    public class Conv_Message
    {
        public int id { get; set; }

        public int conv_id { get; set; }

        public string username { get; set; }

        public string message { get; set; }

        public DateTime mes_fecha { get; set; }
    }
}