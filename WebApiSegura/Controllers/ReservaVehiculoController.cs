using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;
using System.Configuration;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/reservavehiculo")]
    public class ReservaVehiculoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetReserveId(int id)
        {
            ReservaVehiculo reservaVehiculo = new ReservaVehiculo();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_VEH_CODIGO, USU_CODIGO, PAQ_VEH_CODIGO, RES_VEH_FEC_SALIDA, RES_VEH_FEC_REGRESO FROM RESERVA_VEHICULO WHERE RES_VEH_CODIGO = @RES_VEH_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@RES_VEH_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        reservaVehiculo.RES_VEH_CODIGO = sqlDataReader.GetInt32(0);
                        reservaVehiculo.USU_CODIGO = sqlDataReader.GetInt32(1);
                        reservaVehiculo.PAQ_VEH_CODIGO = sqlDataReader.GetInt32(2);
                        reservaVehiculo.RES_VEH_FEC_SALIDA = sqlDataReader.GetDateTime(3);
                        reservaVehiculo.RES_VEH_FEC_REGRESO = sqlDataReader.GetDateTime(4);
                    }
                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }

            return Ok(reservaVehiculo);
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<ReservaVehiculo> reservaVehiculos = new List<ReservaVehiculo>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_VEH_CODIGO, USU_CODIGO, PAQ_VEH_CODIGO, RES_VEH_FEC_SALIDA, RES_VEH_FEC_REGRESO FROM RESERVA_VEHICULO", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        ReservaVehiculo reservaVehiculo = new ReservaVehiculo()
                        {
                            RES_VEH_CODIGO = sqlDataReader.GetInt32(0),
                            USU_CODIGO = sqlDataReader.GetInt32(1),
                            PAQ_VEH_CODIGO = sqlDataReader.GetInt32(2),
                            RES_VEH_FEC_SALIDA = sqlDataReader.GetDateTime(3),
                            RES_VEH_FEC_REGRESO = sqlDataReader.GetDateTime(4),
                            
                        };
                        reservaVehiculos.Add(reservaVehiculo);
                    }


                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(reservaVehiculos);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(ReservaVehiculo reserva)
        {
            if (reserva == null)
                return BadRequest();
            if (RegistrarReservaVehiculo(reserva))
                return Ok(reserva);
            else return InternalServerError();
        }
        private bool RegistrarReservaVehiculo(ReservaVehiculo reserva)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVA_VEHICULO (USU_CODIGO, PAQ_VEH_CODIGO, RES_VEH_FEC_SALIDA, RES_VEH_FEC_REGRESO) VALUES (@USU_CODIGO, @PAQ_VEH_CODIGO, @RES_VEH_FEC_SALIDA, @RES_VEH_FEC_REGRESO)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reserva.USU_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAQ_VEH_CODIGO", reserva.PAQ_VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_FEC_SALIDA", reserva.RES_VEH_FEC_SALIDA);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_FEC_REGRESO", reserva.RES_VEH_FEC_REGRESO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Actualizar(ReservaVehiculo reserva)
        {
            if (reserva == null)
                return BadRequest();
            if (ActualizarReservaVehiculo(reserva))
                return Ok(reserva);
            else return InternalServerError();
        }

        private bool ActualizarReservaVehiculo(ReservaVehiculo reserva)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE RESERVA_VEHICULO 
                                                       SET 
                                                           USU_CODIGO = @USU_CODIGO, 
                                                           PAQ_VEH_CODIGO = @PAQ_VEH_CODIGO,
                                                           RES_VEH_FEC_SALIDA = @RES_VEH_FEC_SALIDA, 
                                                           RES_VEH_FEC_REGRESO = @RES_VEH_FEC_REGRESO
                                                       WHERE RES_VEH_CODIGO = @RES_VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_CODIGO", reserva.RES_VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reserva.USU_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAQ_VEH_CODIGO", reserva.PAQ_VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_FEC_SALIDA", reserva.RES_VEH_FEC_SALIDA);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_FEC_REGRESO", reserva.RES_VEH_FEC_REGRESO);


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
            if (EliminarReservaVehiculo(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarReservaVehiculo(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE RESERVA_VEHICULO 
                                                       WHERE RES_VEH_CODIGO = @RES_VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@RES_VEH_CODIGO", id);


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
