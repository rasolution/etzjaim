using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Cita_Usuario
    {
        public int cita_id { get; set; }

        public string username { get; set; }

        public string user_info { get; set; }

        public DateTime cita_fecha { get; set; }

        public int cita_estado { get; set; }
    }
}