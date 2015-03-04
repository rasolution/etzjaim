using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Cita
    {
        public int cita_id { set; get; }

        public string username { set; get; }

        public DateTime cita_fecha { set; get; }

        public int cita_estado { get; set; }
    }
}