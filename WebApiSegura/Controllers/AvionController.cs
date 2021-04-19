using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/Avion")]
    public class AvionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Avion avion = new Avion();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AVI_CODIGO, AER_CODIGO, AVI_CANT_ASIENTOS, AVI_MODELO, AVI_ESTADO, AVI_DESCRIPCION FROM AVION WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        avion.AVI_CODIGO = sqlDataReader.GetInt32(0);
                        avion.AER_CODIGO = sqlDataReader.GetInt32(1);
                        avion.AVI_CANT_ASIENTOS = sqlDataReader.GetInt32(2);
                        avion.AVI_MODELO = sqlDataReader.GetString(3);
                        avion.AVI_ESTADO = sqlDataReader.GetString(4);
                        avion.AVI_DESCRIPCION = sqlDataReader.GetString(5);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(avion);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Avion> aviones = new List<Avion>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AVI_CODIGO, AER_CODIGO, AVI_CANT_ASIENTOS, AVI_MODELO, AVI_ESTADO, AVI_DESCRIPCION FROM AVION", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Avion avion = new Avion()
                        {
                            AVI_CODIGO = sqlDataReader.GetInt32(0),
                            AER_CODIGO = sqlDataReader.GetInt32(1),
                            AVI_CANT_ASIENTOS = sqlDataReader.GetInt32(2),
                            AVI_MODELO = sqlDataReader.GetString(3),
                            AVI_ESTADO = sqlDataReader.GetString(4),
                            AVI_DESCRIPCION = sqlDataReader.GetString(5)
                    };
                        aviones.Add(avion);
                    }

                    
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
            return Ok(aviones);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Avion avion)
        {
            if (avion == null)
                return BadRequest();
            if (RegistrarAvion(avion))
                return Ok(avion);
            else return InternalServerError();
        }

        private bool RegistrarAvion(Avion avion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AVION (AER_CODIGO, AVI_CANT_ASIENTOS, AVI_MODELO, AVI_ESTADO, AVI_DESCRIPCION) VALUES (@AER_CODIGO, @AVI_CANT_ASIENTOS, @AVI_MODELO, @AVI_ESTADO, @AVI_DESCRIPCION)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", avion.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AVI_CANT_ASIENTOS", avion.AVI_CANT_ASIENTOS);
                sqlCommand.Parameters.AddWithValue("@AVI_MODELO", avion.AVI_MODELO);
                sqlCommand.Parameters.AddWithValue("@AVI_ESTADO", avion.AVI_ESTADO);
                sqlCommand.Parameters.AddWithValue("@AVI_DESCRIPCION", avion.AVI_DESCRIPCION);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }


        [HttpPut]
        public IHttpActionResult Actualizar(Avion avion)
        {
            if (avion == null)
                return BadRequest();
            if (ActualizarAvion(avion))
                return Ok(avion);
            else return InternalServerError();
        }

        private bool ActualizarAvion(Avion avion)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE AVION 
                                                       SET 
                                                           AER_CODIGO = @AER_CODIGO, 
                                                           AVI_CANT_ASIENTOS = @AVI_CANT_ASIENTOS,
                                                           AVI_MODELO = @AVI_MODELO, 
                                                           AVI_ESTADO = @AVI_ESTADO,
                                                           AVI_DESCRIPCION = @AVI_DESCRIPCION
                                                       WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", avion.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", avion.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AVI_CANT_ASIENTOS", avion.AVI_CANT_ASIENTOS);
                sqlCommand.Parameters.AddWithValue("@AVI_MODELO", avion.AVI_MODELO);
                sqlCommand.Parameters.AddWithValue("@AVI_ESTADO", avion.AVI_ESTADO);
                sqlCommand.Parameters.AddWithValue("@AVI_DESCRIPCION", avion.AVI_DESCRIPCION);

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
            if (id<1)
                return BadRequest();
            if (EliminarAvion(id))
                return Ok(id);
            else return InternalServerError();
        }

        private bool EliminarAvion(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE AVION 
                                                       WHERE AVI_CODIGO = @AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", id);


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
