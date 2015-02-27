using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using WebService.Conexion.PostGresSQL;
using WebService.Estructuras;
namespace WebService.Metodos.PostSQL.PostDatabase
{
    public class ProductosPost
    {
        public bool IsError { get; set; }

        public string ErrorDescripcion { get; set; }

        #region guardarProducto
        public void guardarProducto(string pro_nombre, int pro_precio, int pro_estado, byte[] pro_foto)
        {
            pro_nombre = pro_nombre.ToLower();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into productos(pro_nombre,pro_precio,pro_estado,pro_foto) values(@pro_nombre,@pro_precio,@pro_estado,@pro_foto)");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter{
                    ParameterName="pro_nombre",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue =  pro_nombre
                },
                new NpgsqlParameter{
                    ParameterName ="pro_precio",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_precio
                },
                 new NpgsqlParameter{
                    ParameterName ="pro_estado",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_estado
                },
                 new NpgsqlParameter{
                    ParameterName ="pro_foto",
                    NpgsqlDbType=NpgsqlDbType.Bytea,
                    NpgsqlValue=pro_foto
                }
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region eliminarProducto
        public void eliminarProducto(int pro_id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("delete from productos where pro_id=@pro_id");
            var parametros = new List<NpgsqlParameter>{
                new NpgsqlParameter{
                    ParameterName="pro_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_id,
                }
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }

        #endregion

        #region editarProducto
        public void editarProducto(int pro_id, string pro_nombre, int pro_precio, int pro_estado, byte[] pro_foto)
        {
            pro_nombre = pro_nombre.ToLower();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update productos set pro_nombre=@pro_nombre,pro_precio=@pro_precio,pro_estado=@pro_estado,pro_foto=@pro_foto where pro_id=@pro_id");
            var parametros = new List<NpgsqlParameter>
            {
                new NpgsqlParameter
                {
                    ParameterName="pro_id",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_id
                },
                new NpgsqlParameter{
                    ParameterName="pro_nombre",
                    NpgsqlDbType=NpgsqlDbType.Varchar,
                    NpgsqlValue =  pro_nombre
                },
                new NpgsqlParameter{
                    ParameterName ="pro_precio",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_precio
                },
                 new NpgsqlParameter{
                    ParameterName ="pro_estado",
                    NpgsqlDbType=NpgsqlDbType.Integer,
                    NpgsqlValue=pro_estado
                },
                 new NpgsqlParameter{
                    ParameterName ="pro_foto",
                    NpgsqlDbType=NpgsqlDbType.Bytea,
                    NpgsqlValue=pro_foto
                }
            };
            AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString(), parametros);
            if (AccesoDatosPost.Instance.accesodatos.IsError)
            {
                this.IsError = AccesoDatosPost.Instance.accesodatos.IsError;
                this.ErrorDescripcion = AccesoDatosPost.Instance.accesodatos.ErrorDescripcion;
            }
        }
        #endregion

        #region cargarProductos
        public List<Producto> cargarProductos()
        {
            List<Producto> Productos = new List<Producto>();
            var sql = new StringBuilder();
            sql.AppendLine("select * from productos");
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Productos.Add(new Producto()
                {
                    pro_id = Convert.ToInt32(item["pro_id"]),
                    pro_nombre = item["pro_nombre"].ToString(),
                    pro_precio = Convert.ToInt32(item["pro_precio"]),
                    pro_estado = Convert.ToInt32(item["pro_estado"]),
                    pro_foto =GetBytes(item["pro_foto"].ToString()),
                });
            }
            return Productos;
        }
        #endregion

        #region cargarProducto
        public Producto cargarProducto(int pro_id)
        {
            Producto Producto = new Producto();
            var sql = new StringBuilder();
            sql.AppendLine("select * from productos");
            var odatos = AccesoDatosPost.Instance.accesodatos.EjecutarConsultaSQL(sql.ToString());
            foreach (DataRow item in odatos.Tables[0].Rows)
            {
                Producto.pro_id = Convert.ToInt32(item["pro_id"]);
                Producto.pro_nombre = item["pro_nombre"].ToString();
                Producto.pro_precio = Convert.ToInt32(item["pro_precio"]);
                Producto.pro_estado = Convert.ToInt32(item["pro_estado"]);
                Producto.pro_foto = GetBytes(item["pro_foto"].ToString());
            }
            return Producto;
        }
        #endregion

        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
