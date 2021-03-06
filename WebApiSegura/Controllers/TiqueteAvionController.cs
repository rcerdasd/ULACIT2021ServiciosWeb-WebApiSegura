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
    [RoutePrefix("api/tiqueteavion")]
    public class TiqueteAvionController : ApiController
    {
        public IHttpActionResult GetTiqueteByID(int id)
        {
            Tiquete_Avion tiquete_Avion = new Tiquete_Avion();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TIQ_AVI_CODIGO, RES_AVI_CODIGO, TIQ_AVI_ORIGEN, TIQ_AVI_DESTINO, TIQ_AVI_PRECIO
                        FROM            TIQUETE_AVION WHERE TIQ_AVI_CODIGO=@TIQ_AVI_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TIQ_AVI_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        tiquete_Avion.TIQ_AVI_CODIGO = sqlDataReader.GetInt32(0);
                        tiquete_Avion.RES_AVI_CODIGO = sqlDataReader.GetInt32(1);
                        tiquete_Avion.TIQ_AVI_ORIGEN = sqlDataReader.GetString(2);
                        tiquete_Avion.TIQ_AVI_DESTINO = sqlDataReader.GetString(3);
                        tiquete_Avion.TIQ_AVI_PRECIO = sqlDataReader.GetDecimal(4);

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(tiquete_Avion);
        }

        [HttpGet]
        public IHttpActionResult GetALLTiquete()
        {
            List<Tiquete_Avion> tiquete_Avions = new List<Tiquete_Avion>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT TIQ_AVI_CODIGO, RES_AVI_CODIGO, TIQ_AVI_ORIGEN, TIQ_AVI_DESTINO, TIQ_AVI_PRECIO
                                                           FROM            TIQUETE_AVION", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Tiquete_Avion tiquete_Avion = new Tiquete_Avion()
                        {
                            TIQ_AVI_CODIGO = sqlDataReader.GetInt32(0),
                            RES_AVI_CODIGO = sqlDataReader.GetInt32(1),
                            TIQ_AVI_ORIGEN = sqlDataReader.GetString(2),
                            TIQ_AVI_DESTINO = sqlDataReader.GetString(3),
                            TIQ_AVI_PRECIO = sqlDataReader.GetDecimal(4)

                        };
                        tiquete_Avions.Add(tiquete_Avion);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(tiquete_Avions);




        }

        [HttpPost]
        public IHttpActionResult ADD(Tiquete_Avion tiquete_Avion)
        {
            if (tiquete_Avion == null)
                return BadRequest();
            if (AddTiqueteAvion(tiquete_Avion))
                return Ok(tiquete_Avion);
            else
                return InternalServerError();
        }
        private bool AddTiqueteAvion(Tiquete_Avion tiquete_Avion)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO TIQUETE_AVION (  RES_AVI_CODIGO, TIQ_AVI_ORIGEN, TIQ_AVI_DESTINO, TIQ_AVI_PRECIO) VALUES(  @RES_AVI_CODIGO, @TIQ_AVI_ORIGEN, @TIQ_AVI_DESTINO, @TIQ_AVI_PRECIO)", sqlConnection);
                sqlCommand.Parameters.AddWithValue(" @RES_AVI_CODIGO", tiquete_Avion.RES_AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_ORIGEN", tiquete_Avion.TIQ_AVI_ORIGEN);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_DESTINO", tiquete_Avion.TIQ_AVI_DESTINO);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_PRECIO", tiquete_Avion.TIQ_AVI_PRECIO);


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


        [HttpPut]
        public IHttpActionResult Update(Tiquete_Avion tiquete_Avion)
        {
            if (tiquete_Avion == null)
                return BadRequest();
            if (UpdateTiqueteAvion(tiquete_Avion))
                return Ok(tiquete_Avion);
            else
                return InternalServerError();
        }
        private bool UpdateTiqueteAvion(Tiquete_Avion tiquete_Avion)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE TIQUETE_AVION SET
                                                       RES_AVI_CODIGO=@RES_AVI_CODIGO,
                                                       TIQ_AVI_ORIGEN=@TIQ_AVI_ORIGEN,
                                                        TIQ_AVI_DESTINO=@TIQ_AVI_DESTINO,
                                                        TIQ_AVI_PRECIO=@TIQ_AVI_PRECIO,
                                                        WHERE  TIQ_AVI_CODIGO=@TIQ_AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_CODIGO", tiquete_Avion.TIQ_AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue(" @RES_AVI_CODIGO", tiquete_Avion.RES_AVI_CODIGO);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_ORIGEN", tiquete_Avion.TIQ_AVI_ORIGEN);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_DESTINO", tiquete_Avion.TIQ_AVI_DESTINO);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_PRECIO", tiquete_Avion.TIQ_AVI_PRECIO);


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

        public IHttpActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest();
            if (DeleteTiqueteAvion(id))
                return Ok(id);
            else
                return InternalServerError();
        }
        private bool DeleteTiqueteAvion(int id)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE  TIQUETE_AVION  
                                                        WHERE TIQ_AVI_CODIGO=@TIQ_AVI_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TIQ_AVI_CODIGO", id);


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









    }
}
