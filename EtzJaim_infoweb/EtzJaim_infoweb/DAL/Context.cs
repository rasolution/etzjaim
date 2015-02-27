using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EtzJaim_infoweb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EtzJaim_infoweb.DAL
{
    public class Context : DbContext
    {
       /* public Context() : base("Context")
        {
        }
        */

        public DbSet<Terapias> Terapias { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Clientes> Clientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}