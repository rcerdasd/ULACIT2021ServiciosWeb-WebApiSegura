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
    //[Authorize]
    [AllowAnonymous]
    [RoutePrefix("api/paquete-vehiculo")]
    public class PaqueteVehiculoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetPaqueteId(int id)
        {
            PaqueteVehiculo paqueteVehiculo = new PaqueteVehiculo();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PAQ_VEH_CODIGO, VEH_CODIGO, PAQ_SEGURO, PAQ_BICICLETA, PAQ_DESCRIPCION FROM PAQUETE_VEHICULO WHERE PAQ_VEH_CODIGO = @PAQ_VEH_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@PAQ_VEH_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        paqueteVehiculo.PAQ_VEH_CODIGO = sqlDataReader.GetInt32(0);
                        paqueteVehiculo.VEH_CODIGO = sqlDataReader.GetInt32(1);
                        paqueteVehiculo.PAQ_SEGURO = sqlDataReader.GetString(2);
                        paqueteVehiculo.PAQ_BICICLETA = sqlDataReader.GetString(3);
                        paqueteVehiculo.PAQ_DESCRIPCION = sqlDataReader.GetString(4);
                    }
                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }

            return Ok(paqueteVehiculo);
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<PaqueteVehiculo> paqueteVehiculos = new List<PaqueteVehiculo>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PAQ_VEH_CODIGO, VEH_CODIGO, PAQ_SEGURO, PAQ_BICICLETA, PAQ_DESCRIPCION FROM PAQUETE_VEHICULO", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        PaqueteVehiculo paqueteVehiculo = new PaqueteVehiculo()
                        {
                            PAQ_VEH_CODIGO = sqlDataReader.GetInt32(0),
                            VEH_CODIGO = sqlDataReader.GetInt32(1),
                            PAQ_SEGURO = sqlDataReader.GetString(2),
                            PAQ_BICICLETA = sqlDataReader.GetString(3),
                            PAQ_DESCRIPCION = sqlDataReader.GetString(4),

                        };
                        paqueteVehiculos.Add(paqueteVehiculo);
                    }


                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(paqueteVehiculos);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(PaqueteVehiculo paquete)
        {
            if (paquete == null)
                return BadRequest();
            if (RegistrarPaqueteVehiculo(paquete))
                return Ok(paquete);
            else return InternalServerError();
        }
        private bool RegistrarPaqueteVehiculo(PaqueteVehiculo paquete)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO PAQUETE_VEHICULO (VEH_CODIGO, PAQ_SEGURO, PAQ_BICICLETA, PAQ_DESCRIPCION) VALUES (@VEH_CODIGO, @PAQ_SEGURO, @PAQ_BICICLETA, @PAQ_DESCRIPCION)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@VEH_CODIGO", paquete.VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAQ_SEGURO", paquete.PAQ_SEGURO);
                sqlCommand.Parameters.AddWithValue("@PAQ_BICICLETA", paquete.PAQ_BICICLETA);
                sqlCommand.Parameters.AddWithValue("@PAQ_DESCRIPCION", paquete.PAQ_DESCRIPCION);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Actualizar(PaqueteVehiculo paquete)
        {
            if (paquete == null)
                return BadRequest();
            if (ActualizarPaqueteVehiculo(paquete))
                return Ok(paquete);
            else return InternalServerError();
        }

        private bool ActualizarPaqueteVehiculo(PaqueteVehiculo paquete)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand(@"UPDATE PAQUETE_VEHICULO 
                                                       SET 
                                                           VEH_CODIGO = @VEH_CODIGO, 
                                                           PAQ_SEGURO = @PAQ_SEGURO,
                                                           PAQ_BICICLETA = @PAQ_BICICLETA, 
                                                           PAQ_DESCRIPCION = @PAQ_DESCRIPCION
                                                       WHERE PAQ_VEH_CODIGO = @PAQ_VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@PAQ_VEH_CODIGO", paquete.PAQ_VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VEH_CODIGO", paquete.VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PAQ_SEGURO", paquete.PAQ_SEGURO);
                sqlCommand.Parameters.AddWithValue("@PAQ_BICICLETA", paquete.PAQ_BICICLETA);
                sqlCommand.Parameters.AddWithValue("@PAQ_DESCRIPCION", paquete.PAQ_DESCRIPCION);


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
            if (EliminarPaqueteVehiculo(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarPaqueteVehiculo(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE PAQUETE_VEHICULO 
                                                       WHERE PAQ_VEH_CODIGO = @PAQ_VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@PAQ_VEH_CODIGO", id);


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
