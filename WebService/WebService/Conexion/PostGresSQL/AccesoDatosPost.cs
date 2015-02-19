using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace WebService.Conexion.PostGresSQL
{
    public class AccesoDatosPost
    {
        private static readonly Lazy<AccesoDatosPost> instance = new Lazy<AccesoDatosPost>(() => new AccesoDatosPost());
        public IAccesoDatos accesodatos;
        public AccesoDatosPost()
        {
            accesodatos= new AccesoDatosPostgreSql(
                         ConfigurationManager.AppSettings["ServerPG"],
                         ConfigurationManager.AppSettings["PuertoPG"],
                         ConfigurationManager.AppSettings["UsuarioPG"],
                         ConfigurationManager.AppSettings["PasswordPG"],
                         ConfigurationManager.AppSettings["DatabasePG"]
                         );
        }
        public static AccesoDatosPost Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}