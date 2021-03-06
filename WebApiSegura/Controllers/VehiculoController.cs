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
    [RoutePrefix("api/vehiculo")]
    public class VehiculoController : ApiController
    {
        //Metodos Vehiculo
        [HttpGet]
        public IHttpActionResult GetVehId(int id)
        {
            Vehiculo vehiculo = new Vehiculo();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT VEH_CODIGO, REN_CODIGO, VEH_CANT_PASAJEROS, VEH_MODELO, VEH_ESTADO, VEH_DESCRIPCION FROM VEHICULO WHERE VEH_CODIGO = @VEH_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@VEH_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        vehiculo.VEH_CODIGO = sqlDataReader.GetInt32(0);
                        vehiculo.REN_CODIGO = sqlDataReader.GetInt32(1);
                        vehiculo.VEH_CANT_PASAJEROS = sqlDataReader.GetInt32(2);
                        vehiculo.VEH_MODELO = sqlDataReader.GetString(3);
                        vehiculo.VEH_ESTADO = sqlDataReader.GetString(4);
                        vehiculo.VEH_DESCRIPCION = sqlDataReader.GetString(5);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(vehiculo);
        }

        [HttpGet]
        public IHttpActionResult GetVehAll()
        {
            List<Vehiculo> vehiculos = new List<Vehiculo>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT VEH_CODIGO, REN_CODIGO, VEH_CANT_PASAJEROS, VEH_MODELO, VEH_ESTADO, VEH_DESCRIPCION FROM VEHICULO", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Vehiculo vehiculo = new Vehiculo()
                        {
                            VEH_CODIGO = sqlDataReader.GetInt32(0),
                            REN_CODIGO = sqlDataReader.GetInt32(1),
                            VEH_CANT_PASAJEROS = sqlDataReader.GetInt32(2),
                            VEH_MODELO = sqlDataReader.GetString(3),
                            VEH_ESTADO = sqlDataReader.GetString(4),
                            VEH_DESCRIPCION = sqlDataReader.GetString(5)
                        };
                        vehiculos.Add(vehiculo);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(vehiculos);
        }

        [HttpPost]
        public IHttpActionResult IngresarVeh(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                return BadRequest();
            if (RegistrarVehiculo(vehiculo))
                return Ok(vehiculo);
            else return InternalServerError();
        }

        private bool RegistrarVehiculo(Vehiculo vehiculo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO VEHICULO (REN_CODIGO, VEH_CANT_PASAJEROS, VEH_MODELO, VEH_ESTADO, VEH_DESCRIPCION) VALUES (@REN_CODIGO, @VEH_CANT_PASAJEROS, @VEH_MODELO, @VEH_ESTADO, @VEH_DESCRIPCION)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@REN_CODIGO", vehiculo.REN_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VEH_CANT_PASAJEROS", vehiculo.VEH_CANT_PASAJEROS);
                sqlCommand.Parameters.AddWithValue("@VEH_MODELO", vehiculo.VEH_MODELO);
                sqlCommand.Parameters.AddWithValue("@VEH_ESTADO", vehiculo.VEH_ESTADO);
                sqlCommand.Parameters.AddWithValue("@VEH_DESCRIPCION", vehiculo.VEH_DESCRIPCION);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult ActualizarVeh(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                return BadRequest();
            if (ActualizarVehiculo(vehiculo))
                return Ok(vehiculo);
            else return InternalServerError();
        }

        private bool ActualizarVehiculo(Vehiculo vehiculo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE VEHICULO 
                                                       SET 
                                                           REN_CODIGO = @REN_CODIGO, 
                                                           VEH_CANT_PASAJEROS = @VEH_CANT_PASAJEROS,
                                                           VEH_MODELO = @VEH_MODELO, 
                                                           VEH_ESTADO = @VEH_ESTADO,
                                                           VEH_DESCRIPCION = @VEH_DESCRIPCION
                                                       WHERE VEH_CODIGO = @VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@VEH_CODIGO", vehiculo.VEH_CODIGO);
                sqlCommand.Parameters.AddWithValue("@REN_CODIGO", vehiculo.REN_CODIGO);
                sqlCommand.Parameters.AddWithValue("@VEH_CANT_PASAJEROS", vehiculo.VEH_CANT_PASAJEROS);
                sqlCommand.Parameters.AddWithValue("@VEH_MODELO", vehiculo.VEH_MODELO);
                sqlCommand.Parameters.AddWithValue("@VEH_ESTADO", vehiculo.VEH_ESTADO);
                sqlCommand.Parameters.AddWithValue("@VEH_DESCRIPCION", vehiculo.VEH_DESCRIPCION);
                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult EliminarVeh(int id)
        {
            if (id < 1)
                return BadRequest();
            if (EliminarVehiculo(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarVehiculo(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE VEHICULO 
                                                       WHERE VEH_CODIGO = @VEH_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@VEH_CODIGO", id);

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
