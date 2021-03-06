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
    //[Authorize]
    [AllowAnonymous]
    [RoutePrefix("api/reserva-hotel")]
    public class ReservaHotelController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Reserva resHotel = new Reserva();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_CODIGO, USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA FROM RESERVA WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@RES_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        resHotel.RES_CODIGO = sqlDataReader.GetInt32(0);
                        resHotel.USU_CODIGO = sqlDataReader.GetInt32(1);
                        resHotel.HAB_CODIGO = sqlDataReader.GetInt32(2);
                        resHotel.RES_FECHA_INGRESO = sqlDataReader.GetDateTime(3);
                        resHotel.RES_FECHA_SALIDA = sqlDataReader.GetDateTime(4);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(resHotel);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Reserva> reservas = new List<Reserva>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_CODIGO, USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA FROM RESERVA", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Reserva reserva = new Reserva()
                        {
                            RES_CODIGO = sqlDataReader.GetInt32(0),
                            USU_CODIGO = sqlDataReader.GetInt32(1),
                            HAB_CODIGO = sqlDataReader.GetInt32(2),
                            RES_FECHA_INGRESO = sqlDataReader.GetDateTime(3),
                            RES_FECHA_SALIDA = sqlDataReader.GetDateTime(4)
                        };
                        reservas.Add(reserva);
                    }


                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(reservas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Reserva reserva)
        {
            if (reserva == null)
                return BadRequest();
            if (RegistrarReservaHotel(reserva))
                return Ok(reserva);
            else return InternalServerError();
        }
        private bool RegistrarReservaHotel(Reserva reserva)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVA (USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA) VALUES (@USU_CODIGO, @HAB_CODIGO, @RES_FECHA_INGRESO, @RES_FECHA_SALIDA)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@HOT_NOMBRE", reserva.USU_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HOT_EMAIL", reserva.HAB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HOT_DIRECCION", reserva.RES_FECHA_INGRESO);
                sqlCommand.Parameters.AddWithValue("@HOT_TELEFONO", reserva.RES_FECHA_SALIDA);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Reserva reserva)
        {
            if (reserva == null)
                return BadRequest();
            if (ActualizarReserva(reserva))
                return Ok(reserva);
            else return InternalServerError();
        }

        private bool ActualizarReserva(Reserva reserva)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE RESERVA 
                                                       SET 
                                                           USU_CODIGO = @USU_CODIGO, 
                                                           HAB_CODIGO = @HAB_CODIGO,
                                                           RES_FECHA_INGRESO = @RES_FECHA_INGRESO, 
                                                           RES_FECHA_SALIDA = @RES_FECHA_SALIDA,
                                                           HOT_CATEGORIA = @HOT_CATEGORIA
                                                       WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@RES_CODIGO", reserva.RES_CODIGO);
                sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reserva.USU_CODIGO);
                sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", reserva.HAB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@RES_FECHA_INGRESO", reserva.RES_FECHA_INGRESO);
                sqlCommand.Parameters.AddWithValue("@RES_FECHA_SALIDA", reserva.RES_FECHA_SALIDA);
                

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
            if (EliminarReserva(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarReserva(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE RESERVA 
                                                       WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@RES_CODIGO", id);


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
