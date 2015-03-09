using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EtzJaimWebPage.Models
{
    public class Mensajes
    {
        public int mes_id { get; set; }

        public int conv_id { get; set; }

        public string mes_message { get; set; }

        public DateTime mes_salida { get; set; }

    }
}