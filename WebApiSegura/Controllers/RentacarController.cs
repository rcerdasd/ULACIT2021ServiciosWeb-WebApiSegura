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
    [RoutePrefix("api/rentacar")]
    public class RentacarController : ApiController
    {
        //Metodos Rentacar
        [HttpGet]
        public IHttpActionResult GetRentId(int id)
        {
            Rentacar rentacar = new Rentacar();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT REN_CODIGO, REN_NOMBRE, REN_PAIS, REN_TELEFONO, REN_EMAIL FROM RENTACAR WHERE REN_CODIGO = @REN_CODIGO", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@REN_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        rentacar.REN_CODIGO = sqlDataReader.GetInt32(0);
                        rentacar.REN_NOMBRE = sqlDataReader.GetString(1);
                        rentacar.REN_PAIS = sqlDataReader.GetString(2);
                        rentacar.REN_TELEFONO = sqlDataReader.GetString(3);
                        rentacar.REN_EMAIL = sqlDataReader.GetString(4);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(rentacar);
        }

        [HttpGet]
        public IHttpActionResult GetRentAll()
        {
            List<Rentacar> rentacars = new List<Rentacar>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT REN_CODIGO, REN_NOMBRE, REN_PAIS, REN_TELEFONO, REN_EMAIL FROM RENTACAR", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Rentacar rentacar = new Rentacar()
                        {
                            REN_CODIGO = sqlDataReader.GetInt32(0),
                            REN_NOMBRE = sqlDataReader.GetString(1),
                            REN_PAIS = sqlDataReader.GetString(2),
                            REN_TELEFONO = sqlDataReader.GetString(3),
                            REN_EMAIL = sqlDataReader.GetString(4)
                        };
                        rentacars.Add(rentacar);
                    }


                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(rentacars);
        }

        [HttpPost]
        public IHttpActionResult IngresarRent(Rentacar rentacar)
        {
            if (rentacar == null)
                return BadRequest();
            if (RegistrarRentacar(rentacar))
                return Ok(rentacar);
            else return InternalServerError();
        }

        private bool RegistrarRentacar(Rentacar rentacar)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RENTACAR (REN_NOMBRE, REN_PAIS, REN_TELEFONO, REN_EMAIL) VALUES (@REN_NOMBRE, @REN_PAIS, @REN_TELEFONO, @REN_EMAIL)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@REN_NOMBRE", rentacar.REN_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@REN_PAIS", rentacar.REN_PAIS);
                sqlCommand.Parameters.AddWithValue("@REN_TELEFONO", rentacar.REN_TELEFONO);
                sqlCommand.Parameters.AddWithValue("@REN_EMAIL", rentacar.REN_EMAIL);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult ActualizarRent(Rentacar rentacar)
        {
            if (rentacar == null)
                return BadRequest();
            if (ActualizarRentacar(rentacar))
                return Ok(rentacar);
            else return InternalServerError();
        }

        private bool ActualizarRentacar(Rentacar rentacar)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE RENTACAR 
                                                       SET 
                                                           REN_NOMBRE = @REN_NOMBRE, 
                                                           REN_PAIS = @REN_PAIS,
                                                           REN_TELEFONO = @REN_TELEFONO, 
                                                           REN_EMAIL = @REN_EMAIL,
                                                       WHERE REN_CODIGO = @REN_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@REN_CODIGO", rentacar.REN_CODIGO);
                sqlCommand.Parameters.AddWithValue("@REN_NOMBRE", rentacar.REN_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@REN_PAIS", rentacar.REN_PAIS);
                sqlCommand.Parameters.AddWithValue("@REN_TELEFONO", rentacar.REN_TELEFONO);
                sqlCommand.Parameters.AddWithValue("@REN_EMAIL", rentacar.REN_EMAIL);
                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult EliminarRent(int id)
        {
            if (id < 1)
                return BadRequest();
            if (EliminarRentacar(id))
                return Ok();
            else return InternalServerError();
        }

        private bool EliminarRentacar(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"DELETE RENTACAR 
                                                       WHERE REN_CODIGO = @REN_CODIGO", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@REN_CODIGO", id);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    return true;

                sqlConnection.Close();
            }

            return resultado;
        }

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
                                                           VEH_DESCRIPCION = @VEH_DESCRIPCION,
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