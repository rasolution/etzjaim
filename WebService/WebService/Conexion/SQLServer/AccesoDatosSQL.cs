using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Conexion.SQLServer
{
    public class AccesoDatosSQL
    {
        private static readonly Lazy<AccesoDatosSQL> instance = new Lazy<AccesoDatosSQL>(() => new AccesoDatosSQL());
        public IMetodosConexion Accesar;
        public AccesoDatosSQL()
        {
            Accesar = new ConexionSQL(
                     ConfigurationManager.AppSettings["ServerSQL"],
                     ConfigurationManager.AppSettings["UsuarioSQL"],
                     ConfigurationManager.AppSettings["PasswordSQL"],
                     ConfigurationManager.AppSettings["DatabaseSQL"]
                     );
        }

        public static AccesoDatosSQL Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
    