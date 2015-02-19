using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EtzJaim_infoweb.Models
{
    public class TerapiasModelo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
    }

    public class TerapiasModeloDBContext : DbContext
    {
        public DbSet<TerapiasModelo> TerapiasModelo { get; set; }
    } 
}