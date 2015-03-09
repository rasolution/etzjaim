using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EtzJaimWebPage.Models
{
    public class Cita
    {
        public int cita_id { set; get; }

        public string username { set; get; }

        public string cita_fecha { set; get; }

        public string cita_hora { set; get; }

        public int cita_estado { get; set; }
    }
}