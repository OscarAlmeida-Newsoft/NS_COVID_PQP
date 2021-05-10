using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NS_COVID_Entities.Entities;

namespace NS_COVID_Repositories
{
    public class CorreoRepository
    {
        private SqlConnection conexionSQL;

        public CorreoRepository() {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }

        public List<Correo> getCorreosActivos()
        {
            List<Correo> correos = new List<Correo>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerCorreosActivos", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;


            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                correos.Add(new Correo()
                {
                    
                    CorreoElectronico = reader["CorreoElectronico"].ToString().Trim()

                });
            }

            conexionSQL.Close();

            return correos;
        }
    }
}
