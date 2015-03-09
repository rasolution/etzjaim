using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EtzJaimWebPage.Models
{
    public class Cita_Usuario
    {
        public int cita_id { get; set; }

        public string username { get; set; }

        public string user_info { get; set; }

        public string cita_fecha { set; get; }

        public string cita_hora { set; get; }

        public int cita_estado { get; set; }
    }
}