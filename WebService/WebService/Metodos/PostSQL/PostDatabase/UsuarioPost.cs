using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras;

namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class UsuarioPost
    {
        public bool IsError { get; set; }
        public string ErrorDescripcion { get; set; }

        private static bool HasRows(DataSet ds)
        {
            return ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }

        private static Usuario MapPublicUsuario(DataRow item)
        {
            return new Usuario
            {
                username = item["username"].ToString(),
                password = string.Empty,
                nombre = item["user_nombre"].ToString(),
                apellidos = item["user_apellidos"].ToString(),
                tipo = Convert.ToInt32(item["user_tipo"])
            };
        }

        public List<Usuario> cargarUsuarios()
        {
            var usuarios = new List<Usuario>();
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL("select username, user_nombre, user_apellidos, user_tipo from usuarios where user_tipo < 2");
            if (HasRows(odatos))
            {
                foreach (DataRow item in odatos.Tables[0].Rows)
                {
                    usuarios.Add(MapPublicUsuario(item));
                }
            }
            return usuarios;
        }

        public List<Usuario> cargarAdmins_Clientes(int tipo)
        {
            var usuarios = new List<Usuario>();
            if (tipo != 0 && tipo != 1)
            {
                return usuarios;
            }

            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter
                {
                    ParameterName = "tipo",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    NpgsqlValue = tipo
                }
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(
                "select username, user_nombre, user_apellidos, user_tipo from usuarios where user_tipo = @tipo",
                parametros);

            if (HasRows(odatos))
            {
                foreach (DataRow item in odatos.Tables[0].Rows)
                {
                    usuarios.Add(MapPublicUsuario(item));
                }
            }
            return usuarios;
        }

        public Usuario cargarUsuario(string username)
        {
            Usuario usuario = new Usuario();
            if (string.IsNullOrWhiteSpace(username))
            {
                return usuario;
            }

            string normalized = username.Trim().ToLowerInvariant();
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter
                {
                    ParameterName = "username",
                    NpgsqlDbType = NpgsqlDbType.Varchar,
                    NpgsqlValue = normalized
                }
            };

            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(
                "select username, user_nombre, user_apellidos, user_tipo from usuarios where username = @username",
                parametros);

            if (HasRows(odatos))
            {
                return MapPublicUsuario(odatos.Tables[0].Rows[0]);
            }
            return usuario;
        }

        public void guardarUsuario(string username, string password, string user_nombre, string user_apellidos, int user_tipo)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                IsError = true;
                ErrorDescripcion = "El nombre de usuario y la contraseña son obligatorios.";
                return;
            }

            string normalizedUser = username.Trim().ToLowerInvariant();
            string hashedPass = encriptacion(password.ToLowerInvariant());

            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = normalizedUser },
                new NpgsqlParameter { ParameterName = "password", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = hashedPass },
                new NpgsqlParameter { ParameterName = "user_nombre", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = (user_nombre ?? string.Empty).Trim() },
                new NpgsqlParameter { ParameterName = "user_apellidos", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = (user_apellidos ?? string.Empty).Trim() },
                new NpgsqlParameter { ParameterName = "user_tipo", NpgsqlDbType = NpgsqlDbType.Integer, NpgsqlValue = user_tipo }
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarSQL(
                "insert into usuarios(username,password,user_nombre,user_apellidos,user_tipo) values (@username,@password,@user_nombre,@user_apellidos,@user_tipo)",
                parametros);

            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = true;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        public void eliminarUsuario(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return;
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = username.Trim().ToLowerInvariant() }
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarSQL("update usuarios set user_tipo = 2 where username = @username", parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = true;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        public void habilitarUsuario(string username, int tipo)
        {
            if (string.IsNullOrWhiteSpace(username)) return;
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "tipo", NpgsqlDbType = NpgsqlDbType.Integer, NpgsqlValue = tipo },
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = username.Trim().ToLowerInvariant() }
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarSQL("update usuarios set user_tipo = @tipo where username = @username", parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = true;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        public void cambiarContraseña(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                this.IsError = true;
                this.ErrorDescripcion = "El usuario y la nueva contraseña son obligatorios.";
                return;
            }

            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = username.Trim().ToLowerInvariant() },
                new NpgsqlParameter { ParameterName = "password", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = encriptacion(password.ToLowerInvariant()) }
            };

            AccesoDatosPost.Instance.accesodatos.EjecutarSQL("update usuarios set password = @password where username = @username", parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = true;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        public bool UsernameRepetido(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = username.Trim().ToLowerInvariant() }
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(
                "select username from usuarios where user_tipo < 2 and username = @username",
                parametros);
            return HasRows(odatos);
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            string normalizedUser = username.Trim().ToLowerInvariant();
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter { ParameterName = "username", NpgsqlDbType = NpgsqlDbType.Varchar, NpgsqlValue = normalizedUser }
            };
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(
                "select password from usuarios where user_tipo < 2 and username = @username",
                parametros);

            if (!HasRows(odatos))
            {
                return false;
            }

            string storedHash = odatos.Tables[0].Rows[0]["password"].ToString();
            return VerificarPassword(password, storedHash);
        }

        public List<Usuario> user_Ajax(string character)
        {
            var usuarios = new List<Usuario>();
            string pattern = "%" + (character ?? string.Empty).Trim().ToLowerInvariant() + "%";
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter
                {
                    ParameterName = "pattern",
                    NpgsqlDbType = NpgsqlDbType.Varchar,
                    NpgsqlValue = pattern
                }
            };

            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(
                "select username, user_nombre, user_apellidos, user_tipo from usuarios where user_tipo < 2 and lower(username) like @pattern",
                parametros);

            if (HasRows(odatos))
            {
                foreach (DataRow item in odatos.Tables[0].Rows)
                {
                    usuarios.Add(MapPublicUsuario(item));
                }
            }
            return usuarios;
        }

        public string encriptacion(string password)
        {
            return HashSha256(password ?? string.Empty);
        }

        public static string HashSha256(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
                StringBuilder sb = new StringBuilder("sha256:", bytes.Length * 2 + 7);
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string HashLegacyMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.ASCII.GetBytes(input ?? string.Empty));
                StringBuilder sb = new StringBuilder(data.Length * 2);
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static bool VerificarPassword(string plainPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string normalized = plainPassword.ToLowerInvariant();
            if (storedHash.StartsWith("sha256:", StringComparison.OrdinalIgnoreCase))
            {
                return FixedTimeEquals(HashSha256(normalized), storedHash)
                    || FixedTimeEquals(HashSha256(plainPassword), storedHash);
            }

            return FixedTimeEquals(HashLegacyMd5(normalized), storedHash.ToLowerInvariant());
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
