using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using EtzJaim_infoweb.Models;

namespace EtzJaim_infoweb.DAL
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var Terapias = new List<Terapias>
            {
            new Terapias{ID=01,Name="Energía Molecular",Price=8000,Description="Botellas con Agua en Vibración Energética correcta",Time="Un Mes"},
            new Terapias{ID=02,Name="Quiro-Masaje",Price=10000,Description="Masaje Quiropráctico Corrector",Time="Una Hora"},
            new Terapias{ID=03,Name="Limpieza Iónica",Price=15000,Description="Limpieza de Iones del Cuerpo",Time="Un Mes"},
            };
            Terapias.ForEach(s => context.Terapias.Add(s));
            context.SaveChanges();

            var Productos = new List<Productos>
            {
            new Productos{ID=001,Name="SaniTé",Price=5000,Description="Gotas de Disolución acido-salina cargadas de Plata",Stock=5},
            new Productos{ID=002,Name="Aguas Purificada",Price=10000,Description="Botellas con Agua Mineral con Peróxido de Hidrógeno",Stock=10},
            new Productos{ID=003,Name="Tónico Limpiador",Price=8500,Description="Botellitas con Disolución de Alcohol",Stock=2},
           };
            Productos.ForEach(s => context.Productos.Add(s));
            context.SaveChanges();

            var Clientes = new List<Clientes>
            {
            new Clientes{ID=101230356,Name="Juan",LastName="González",Address="La Trinidad, Moravia, San José",mail="juagonza@hotmail.com",Phone=89763456},
            new Clientes{ID=202340324,Name="María",LastName="Mendoza",Address="Quesada, San Carlos, Alajuela",mail="marimendoz@gmail.com",Phone=24605689},
            new Clientes{ID=305670678,Name="Luis",LastName="Rodriguez",Address="Paraiso, Cartago",mail="lrodriguez@yahoo.com",Phone=69874723},
           };
            Clientes.ForEach(s => context.Clientes.Add(s));
            context.SaveChanges();
        }
    }
}