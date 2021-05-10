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
    public class PreguntaRepository
    {

        private SqlConnection conexionSQL;

        public PreguntaRepository() {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }

        public List<Pregunta> getPreguntasActivas(string formulario)
        {
            List<Pregunta> preguntas = new List<Pregunta>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerPreguntasActivas", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@Formulario", formulario);


            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                preguntas.Add(new Pregunta()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Tipo = reader["Tipo"].ToString().Trim(),
                    TipoValor= reader["TipoValor"].ToString().Trim(),
                    Enunciado = reader["Enunciado"].ToString().Trim(),
                    ValorXDefecto = reader["ValorXDefecto"].ToString().Trim(),
                    GeneraAlerta = Convert.ToBoolean(reader["GeneraAlerta"]),
                    ValorGeneraAlerta = reader["ValorGeneraAlerta"].ToString().Trim(),
                    CondicionGeneraAlerta = reader["CondicionGeneraAlerta"].ToString().Trim(),
                    Orden = Convert.ToInt32(reader["Orden"])
                });
            }

            conexionSQL.Close();

            return preguntas;
        }
    }
}
