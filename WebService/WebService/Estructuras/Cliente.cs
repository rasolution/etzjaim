using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Cliente
    {
        public int cl_id { set; get; }

        public string cl_username { set; get; }

        public string cl_password { set; get; }

        public string cl_nombre { set; get; }

        public string cl_apellidos { set; get; }
    }
}