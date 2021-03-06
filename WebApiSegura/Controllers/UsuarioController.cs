using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Usuario usuario = new Usuario();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT USU_CODIGO, USU_IDENTIFICACION, USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, USU_FEC_NAC, USU_TELEFONO FROM USUARIO WHERE USU_CODIGO = @USU_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        usuario.USU_CODIGO = sqlDataReader.GetInt32(0);
                        usuario.USU_IDENTIFICACION = sqlDataReader.GetString(1);
                        usuario.USU_NOMBRE = sqlDataReader.GetString(2);
                        usuario.USU_PASSWORD = sqlDataReader.GetString(3);
                        usuario.USU_EMAIL = sqlDataReader.GetString(4);
                        usuario.USU_ESTADO = sqlDataReader.GetString(5);
                        usuario.USU_FEC_NAC = sqlDataReader.GetDateTime(6);
                        usuario.USU_TELEFONO = sqlDataReader.GetString(7);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(usuario);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT USU_CODIGO, USU_IDENTIFICACION, USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, USU_FEC_NAC, USU_TELEFONO FROM USUARIO", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            USU_CODIGO = sqlDataReader.GetInt32(0),
                            USU_IDENTIFICACION = sqlDataReader.GetString(1),
                            USU_NOMBRE = sqlDataReader.GetString(2),
                            USU_PASSWORD = sqlDataReader.GetString(3),
                            USU_EMAIL = sqlDataReader.GetString(4),
                            USU_ESTADO = sqlDataReader.GetString(5),
                            USU_FEC_NAC = sqlDataReader.GetDateTime(6),
                            USU_TELEFONO = sqlDataReader.GetString(7)
                        };
                        usuarios.Add(usuario);
                    }


                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(usuarios);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();
            if (RegistrarUsuario(usuario))
                return Ok(usuario);
            else return InternalServerError();
        }
        private bool RegistrarUsuario(Usuario usuario)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO USUARIO (USU_IDENTIFICACION, USU_NOMBRE, USU_PASSWORD, USU_EMAIL, USU_ESTADO, USU_FEC_NAC, USU_TELEFONO) VALUES (@USU_IDENTIFICACION, @USU_NOMBRE, @USU_PASSWORD, @USU_EMAIL, @USU_ESTADO, @USU_FEC_NAC, @USU_TELEFONO)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", usuario.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@USU_NOMBRE", usuario.USU_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@USU_PASSWORD", usuario.USU_PASSWORD);
                sqlCommand.Parameters.AddWithValue("@USU_EMAIL", usuario.USU_EMAIL);
                sqlCommand.Parameters.AddWithValue("@USU_ESTADO", usuario.USU_ESTADO);
                sqlCommand.Parameters.AddWithValue("@USU_FEC_NAC", usuario.USU_FEC_NAC);
                sqlCommand.Parameters.AddWithValue("@USU_TELEFONO", usuario.USU_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest();
            if (ActualizarUsuario(usuario))
                return Ok(usuario);
            else return InternalServerError();
        }

        private bool ActualizarUsuario(Usuario usuario)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE USUARIO 
                                                       SET 
                                                           USU_IDENTIFICACION = @USU_IDENTIFICACION, 
                                                           USU_NOMBRE = @USU_NOMBRE,
                                                           USU_PASSWORD = @USU_PASSWORD, 
                                                           USU_EMAIL = @USU_EMAIL,
                                                           USU_ESTADO = @USU_ESTADO,
                                                           USU_FEC_NAC = @USU_FEC_NAC,
                                                           USU_TELEFONO = @USU_TELEFONO
                                                       WHERE USU_CODIGO = @USU_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@USU_CODIGO", usuario.USU_CODIGO);
                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", usuario.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@USU_NOMBRE", usuario.USU_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@USU_PASSWORD", usuario.USU_PASSWORD);
                sqlCommand.Parameters.AddWithValue("@USU_EMAIL", usuario.USU_EMAIL);
                sqlCommand.Parameters.AddWithValue("@USU_ESTADO", usuario.USU_ESTADO);
                sqlCommand.Parameters.AddWithValue("@USU_FEC_NAC", usuario.USU_FEC_NAC);
                sqlCommand.Parameters.AddWithValue("@USU_TELEFONO", usuario.USU_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id < 1)
                return BadRequest();
            if (EliminarUsuario(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarUsuario(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE USUARIO 
                                                       WHERE USU_CODIGO = @USU_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@USU_CODIGO", id);


                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }
    }
}
