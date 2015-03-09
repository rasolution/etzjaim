using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EtzJaimWebPage.Models
{
    public class MensajeFinal
    {
        public List<Conv_Message> messages { get; set; }

        public Usuario user { get; set; }
    }
}