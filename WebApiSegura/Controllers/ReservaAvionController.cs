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
    [RoutePrefix("api/reservaavion")]
    public class ReservaAvionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetReservaByID(int id)
        {
            ReservaAvion reservaAvion = new ReservaAvion();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_AVI_CODIGO, USU_CODIGO, AVI_CODIGO, RES_AVI_FEC_VUELO, RES_AVI_DURACION, RES_AVI_ESCALA
                        FROM            RESERVA_AVION WHERE RES_AVI_CODIGO=@RES_AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        reservaAvion.RES_AVI_CODIGO = sqlDataReader.GetInt32(0);
                        reservaAvion.USU_CODIGO = sqlDataReader.GetInt32(1);
                        reservaAvion.AVI_CODIGO = sqlDataReader.GetInt32(2);
                        reservaAvion.RES_AVI_FEC_VUELO = sqlDataReader.GetDateTime(3);
                        reservaAvion.RES_AVI_DURACION = sqlDataReader.GetDecimal(4);
                        reservaAvion.RES_AVI_ESCALA = sqlDataReader.GetString(5);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(reservaAvion);
        }
        [HttpGet]
        public IHttpActionResult GetALLRESERVA()
        {
            List<ReservaAvion> reservasAvion = new List<ReservaAvion>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT        RES_AVI_CODIGO, USU_CODIGO, AVI_CODIGO, RES_AVI_FEC_VUELO, RES_AVI_DURACION, RES_AVI_ESCALA
                                                           FROM            RESERVA_AVION", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        ReservaAvion reservaAvion = new ReservaAvion()
                        {
                            RES_AVI_CODIGO = sqlDataReader.GetInt32(0),
                            USU_CODIGO = sqlDataReader.GetInt32(1),
                            AVI_CODIGO = sqlDataReader.GetInt32(2),
                            RES_AVI_FEC_VUELO = sqlDataReader.GetDateTime(3),
                            RES_AVI_DURACION = sqlDataReader.GetDecimal(4),
                            RES_AVI_ESCALA = sqlDataReader.GetString(5)
                        };
                        reservasAvion.Add(reservaAvion);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(reservasAvion);




        }

        [HttpPost]
        public IHttpActionResult ADD(ReservaAvion reservaAvion)
        {
            if (reservaAvion == null)
                return BadRequest();
            if (AddReservaAvion(reservaAvion))
                return Ok(reservaAvion);
            else
                return InternalServerError();
        }
        private bool AddReservaAvion(ReservaAvion reservaAvion)
        {
            try
            {
                bool resultado = false;
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVA_AVION ( USU_CODIGO, AVI_CODIGO, RES_AVI_FEC_VUELO, RES_AVI_DURACION, RES_AVI_ESCALA) VALUES(   @USU_CODIGO, @AVI_CODIGO, @RES_AVI_FEC_VUELO, @RES_AVI_DURACION, @RES_AVI_ESCALA)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reservaAvion.USU_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", reservaAvion.AVI_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_FEC_VUELO", reservaAvion.RES_AVI_FEC_VUELO);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_DURACION", reservaAvion.RES_AVI_DURACION);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_ESCALA", reservaAvion.RES_AVI_ESCALA);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        resultado = true;
                    }

                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [HttpPut]
        public IHttpActionResult Update(ReservaAvion reservaAvion)
        {
            if (reservaAvion == null)
                return BadRequest();
            if (UpdateReservaAvion(reservaAvion))
                return Ok(reservaAvion);
            else
                return InternalServerError();
        }
        private bool UpdateReservaAvion(ReservaAvion reservaAvion)
        {
            try
            {
                bool resultado = false;
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE RESERVA_AVION SET
                                                       USU_CODIGO=@USU_CODIGO,
                                                       AVI_CODIGO=@AVI_CODIGO,
                                                        RES_AVI_FEC_VUELO=@RES_AVI_FEC_VUELO,
                                                        RES_AVI_DURACION=@RES_AVI_DURACION,
                                                        RES_AVI_ESCALA=@RES_AVI_ESCALA
                                                        WHERE  RES_AVI_CODIGO=@RES_AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_CODIGO", reservaAvion.RES_AVI_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reservaAvion.USU_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@AVI_CODIGO", reservaAvion.AVI_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_FEC_VUELO", reservaAvion.RES_AVI_FEC_VUELO);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_DURACION", reservaAvion.RES_AVI_DURACION);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_ESCALA", reservaAvion.RES_AVI_ESCALA);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        resultado = true;
                    }

                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public IHttpActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest();
            if (DeleteReservaAvion(id))
                return Ok(id);
            else
                return InternalServerError();
        }
        private bool DeleteReservaAvion(int id)
        {
            try
            {
                bool resultado = false;
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE  RESERVA_AVION  
                                                        WHERE RES_AVI_CODIGO=@RES_AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@RES_AVI_CODIGO", id);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        resultado = true;
                    }

                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception)
            {

                return false;
            }
        }








    }
}
