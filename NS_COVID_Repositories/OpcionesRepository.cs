using NS_COVID_Entities.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Repositories
{
    public class OpcionesRepository
    {
        private SqlConnection conexionSQL;

        public OpcionesRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }

        public List<Opciones> getOpcionesByPregunta(int id)
        {

            List<Opciones> opciones = new List<Opciones>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerOpcionesPorPregunta", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;

            commando.Parameters.AddWithValue("@IdPregunta", id);

            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                opciones.Add(new Opciones()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    TextoOpcion = reader["TextoOpcion"].ToString().Trim()

                });
            }

            conexionSQL.Close();

            return opciones;

        }
    }
}
