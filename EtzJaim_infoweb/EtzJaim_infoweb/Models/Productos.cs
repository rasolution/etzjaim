using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EtzJaim_infoweb.Models
{
    public class Productos
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}