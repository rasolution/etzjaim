using System;
using System.Configuration;

namespace WebService.Conexion.PostGresSQL
{
    public class PostgresConnectionSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public string BuildConnectionString()
        {
            return string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                Host ?? "localhost",
                Port ?? "5432",
                Database ?? "etzjaim",
                User ?? "etzjaim",
                Password ?? string.Empty);
        }
    }

    public class AccesoDatosPost
    {
        private static readonly Lazy<AccesoDatosPost> instance = new Lazy<AccesoDatosPost>(() => new AccesoDatosPost());
        public IAccesoDatos accesodatos;

        public static PostgresConnectionSettings ResolveSettings()
        {
            return new PostgresConnectionSettings
            {
                Host = GetSetting("ETZJAIM_PG_HOST", "ServerPG", "localhost"),
                Port = GetSetting("ETZJAIM_PG_PORT", "PuertoPG", "5432"),
                User = GetSetting("ETZJAIM_PG_USER", "UsuarioPG", "etzjaim"),
                Password = GetSetting("ETZJAIM_PG_PASSWORD", "PasswordPG", string.Empty),
                Database = GetSetting("ETZJAIM_PG_DB", "DatabasePG", "etzjaim")
            };
        }

        private static string GetSetting(string envKey, string appSettingKey, string fallback)
        {
            string envVal = Environment.GetEnvironmentVariable(envKey);
            if (!string.IsNullOrWhiteSpace(envVal))
            {
                return envVal.Trim();
            }

            string cfgVal = ConfigurationManager.AppSettings[appSettingKey];
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                return cfgVal.Trim();
            }

            return fallback;
        }

        public AccesoDatosPost()
        {
            PostgresConnectionSettings cfg = ResolveSettings();
            accesodatos = new AccesoDatosPostgreSql(cfg.Host, cfg.Port, cfg.User, cfg.Password, cfg.Database);
        }

        public static AccesoDatosPost Instance
        {
            get { return instance.Value; }
        }
    }
}
