using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Usuario
    {
        public string username { get; set; }

        public string password { get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        // 0 Administrador 1 Cliente
        public int tipo { get; set; }
    }
}