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
    [RoutePrefix("api/aerolinea")]
    public class AerolineaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Aerolinea aerolinea = new Aerolinea();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AER_CODIGO, AER_NOMBRE, AER_PAIS, AER_TELEFONO, AER_EMAIL FROM AEROLINEA WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@AER_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        aerolinea.AER_CODIGO = sqlDataReader.GetInt32(0);
                        aerolinea.AER_NOMBRE = sqlDataReader.GetString(1);
                        aerolinea.AER_PAIS = sqlDataReader.GetString(2);
                        aerolinea.AER_TELEFONO = sqlDataReader.GetString(3);
                        aerolinea.AER_EMAIL = sqlDataReader.GetString(4);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aerolinea);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Aerolinea> aerolineas = new List<Aerolinea>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT AER_CODIGO, AER_NOMBRE, AER_PAIS, AER_TELEFONO, AER_EMAIL FROM AEROLINEA", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Aerolinea aerolinea = new Aerolinea()
                        {
                            AER_CODIGO = sqlDataReader.GetInt32(0),
                            AER_NOMBRE = sqlDataReader.GetString(1),
                            AER_PAIS = sqlDataReader.GetString(2),
                            AER_TELEFONO = sqlDataReader.GetString(3),
                            AER_EMAIL = sqlDataReader.GetString(4)
                        };
                        aerolineas.Add(aerolinea);
                    }

                    
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
            return Ok(aerolineas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();
            if (RegistrarAerolinea(aerolinea))
                return Ok(aerolinea);
            else return InternalServerError();
        }

        private bool RegistrarAerolinea(Aerolinea aerolinea)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AEROLINEA (AER_NOMBRE, AER_PAIS, AER_TELEFONO, AER_EMAIL) VALUES (@AER_NOMBRE, @AER_PAIS, @AER_TELEFONO, @AER_EMAIL)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AER_NOMBRE", aerolinea.AER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@AER_PAIS", aerolinea.AER_PAIS);
                sqlCommand.Parameters.AddWithValue("@AER_TELEFONO", aerolinea.AER_TELEFONO);
                sqlCommand.Parameters.AddWithValue("@AER_EMAIL", aerolinea.AER_EMAIL);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }


        [HttpPut]
        public IHttpActionResult Actualizar(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();
            if (ActualizarAerolinea(aerolinea))
                return Ok(aerolinea);
            else return InternalServerError();
        }

        private bool ActualizarAerolinea(Aerolinea aerolinea)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE AEROLINEA 
                                                       SET 
                                                           AER_NOMBRE = @AER_NOMBRE, 
                                                           AER_PAIS = @AER_PAIS,
                                                           AER_TELEFONO = @AER_TELEFONO, 
                                                           AER_EMAIL = @AER_EMAIL
                                                       WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", aerolinea.AER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AER_NOMBRE", aerolinea.AER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@AER_PAIS", aerolinea.AER_PAIS);
                sqlCommand.Parameters.AddWithValue("@AER_TELEFONO", aerolinea.AER_TELEFONO);
                sqlCommand.Parameters.AddWithValue("@AER_EMAIL", aerolinea.AER_EMAIL);

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
            if (EliminarAerolinea(id))
                return Ok(id);
            else return InternalServerError();
        }

        private bool EliminarAerolinea(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE AEROLINEA 
                                                       WHERE AER_CODIGO = @AER_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AER_CODIGO", id);


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
