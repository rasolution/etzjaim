using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Estructuras
{
    public class Producto
    {
        public int pro_id { get; set; }

        public string pro_nombre { get; set; }

        public int pro_precio { get; set; }

        public int pro_estado { get; set; }

        public byte[] pro_foto { get; set; }
    }
}