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
    [RoutePrefix("api/AsientoAvion")]
    public class AsientoAvionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            AsientoAvion asiento = new AsientoAvion();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT ASI_AVI_CODIGO, AVI_CODIGO, ASI_AVI_NUMERO, ASI_AVI_POSICION, ASI_AVI_CLASE, ASI_AVI_PRECIO FROM ASIENTO_AVION WHERE ASI_AVI_CODIGO = @ASI_AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ASI_AVI_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        asiento.ASI_AVI_CODIGO = sqlDataReader.GetInt32(0);
                        asiento.AVI_CODIGO = sqlDataReader.GetInt32(1);
                        asiento.ASI_AVI_NUMERO = sqlDataReader.GetInt32(2);
                        asiento.ASI_AVI_POSICION = sqlDataReader.GetString(3);
                        asiento.ASI_AVI_CLASE = sqlDataReader.GetString(4);
                        asiento.ASI_AVI_PRECIO = sqlDataReader.GetDecimal(5);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(asiento);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<AsientoAvion> asientos = new List<AsientoAvion>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT ASI_AVI_CODIGO, AVI_CODIGO, ASI_AVI_NUMERO, ASI_AVI_POSICION, ASI_AVI_CLASE, ASI_AVI_PRECIO FROM ASIENTO_AVION", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        AsientoAvion asiento = new AsientoAvion()
                        {
                            ASI_AVI_CODIGO = sqlDataReader.GetInt32(0),
                            AVI_CODIGO = sqlDataReader.GetInt32(1),
                            ASI_AVI_NUMERO = sqlDataReader.GetInt32(2),
                            ASI_AVI_POSICION = sqlDataReader.GetString(3),
                            ASI_AVI_CLASE = sqlDataReader.GetString(4),
                            ASI_AVI_PRECIO = sqlDataReader.GetDecimal(5)
                    };
                        asientos.Add(asiento);
                    }

                    
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
            return Ok(asientos);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(AsientoAvion asiento)
        {
            if (asiento == null)
                return BadRequest();
            if (RegistrarAsiento(asiento))
                return Ok(asiento);
            else return InternalServerError();
        }

        private bool RegistrarAsiento(AsientoAvion asiento)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO ASIENTO_AVION (AVI_CODIGO, ASI_AVI_NUMERO, ASI_AVI_POSICION, ASI_AVI_CLASE, ASI_AVI_PRECIO) VALUES (@AVI_CODIGO, @ASI_AVI_NUMERO, @ASI_AVI_POSICION, @ASI_AVI_CLASE, @ASI_AVI_PRECIO)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", asiento.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_NUMERO", asiento.ASI_AVI_NUMERO);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_POSICION", asiento.ASI_AVI_POSICION);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_CLASE", asiento.ASI_AVI_CLASE);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_PRECIO", asiento.ASI_AVI_PRECIO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }


        [HttpPut]
        public IHttpActionResult Actualizar(AsientoAvion asiento)
        {
            if (asiento == null)
                return BadRequest();
            if (ActualizarAsiento(asiento))
                return Ok(asiento);
            else return InternalServerError();
        }

        private bool ActualizarAsiento(AsientoAvion asiento)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE ASIENTO_AVION 
                                                       SET 
                                                           AVI_CODIGO = @AVI_CODIGO, 
                                                           ASI_AVI_NUMERO = @ASI_AVI_NUMERO,
                                                           ASI_AVI_POSICION = @ASI_AVI_POSICION, 
                                                           ASI_AVI_CLASE = @ASI_AVI_CLASE,
                                                           ASI_AVI_PRECIO = @ASI_AVI_PRECIO
                                                       WHERE ASI_AVI_CODIGO = @ASI_AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_CODIGO", asiento.ASI_AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", asiento.AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_NUMERO", asiento.ASI_AVI_NUMERO);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_POSICION", asiento.ASI_AVI_POSICION);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_CLASE", asiento.ASI_AVI_CLASE);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_PRECIO", asiento.ASI_AVI_PRECIO);

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
            if (EliminarAsiento(id))
                return Ok(id);
            else return InternalServerError();
        }

        private bool EliminarAsiento(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE ASIENTO_AVION 
                                                       WHERE ASI_AVI_CODIGO = @ASI_AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ASI_AVI_CODIGO", id);


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
