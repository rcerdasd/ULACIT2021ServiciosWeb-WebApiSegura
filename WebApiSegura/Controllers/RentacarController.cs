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
                                                           REN_EMAIL = @REN_EMAIL
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
                return Ok(id);
            else return InternalServerError();
        }

        private bool EliminarRentacar(int id)
        {
            try
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
            catch (Exception)
            {

                return false;
            }
        }

        
    }
}