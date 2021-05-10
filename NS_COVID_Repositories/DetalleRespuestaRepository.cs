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
    public class DetalleRespuestaRepository
    {

        private SqlConnection conexionSQL;

        public DetalleRespuestaRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }

        public void GuardarDetalleRespuestas(List<DetalleRespuesta> detalleRespuestas, int? idRespuesta, int? idPersona)
        {
            conexionSQL.Open();

            foreach (DetalleRespuesta d in detalleRespuestas) {

                SqlCommand commando = new SqlCommand("COVID_GuardarDetalleRespuesta", conexionSQL);

                commando.CommandType = CommandType.StoredProcedure;

                //TODO: Condicionar IdRespuesta a que tenga un valor diferente de null
                //TODO: Adicionar parametro IdPersona
                if (idRespuesta != null)
                {
                    commando.Parameters.AddWithValue("@IdRespuesta", idRespuesta);
                }
                if (idPersona != null)
                {
                    commando.Parameters.AddWithValue("@IdPersona", idPersona);
                }

                commando.Parameters.AddWithValue("@IdPregunta", d.IdPregunta);
                commando.Parameters.AddWithValue("@ValorRespuesta", d.ValorRespuesta);
                commando.Parameters.AddWithValue("@GeneroAlerta", d.GeneroAlerta);


                commando.ExecuteNonQuery();

            }

            


            conexionSQL.Close();
        }

        public List<DetalleRespuesta> getDetalleRespustasPorId(int IdRespuesta)
        {

            List<DetalleRespuesta> detalles = new List<DetalleRespuesta>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerDetalleRespuestaPorId", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@IdRespuesta", IdRespuesta);

            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                DetalleRespuesta det = new DetalleRespuesta();
                det.Pregunta = new Pregunta();

                det.GeneroAlerta = Convert.ToBoolean(reader["GeneroAlerta"]);
                det.ValorRespuesta = reader["GeneroAlerta"].ToString();
                det.Pregunta.Descripcion = reader["Descripcion"].ToString();
                det.Pregunta.Enunciado = reader["Enunciado"].ToString();

                detalles.Add(det);
            }

            conexionSQL.Close();

            return detalles;
        }
    }
}
