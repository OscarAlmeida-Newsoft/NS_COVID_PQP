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
    public class RespuestaRepository
    {

        private SqlConnection conexionSQL;

        public RespuestaRepository() {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }


        public List<Respuesta> getRespuestasXUsuarioXDia(int IdPersona) {

            List<Respuesta> respuestas = new List<Respuesta>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtieneRegistrosDelDiaPorPersona", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@IdPersona", IdPersona);

            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                respuestas.Add(new Respuesta()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Tipo = reader["Tipo"].ToString().Trim()                    

                });
            }

            conexionSQL.Close();

            

            return respuestas;
        }

        public int GuardarRespuesta(Respuesta miRespuesta)
        {
            int IdRespuesta = 0;
            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_GuardarRespuesta", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@IdPersona", miRespuesta.IdPersona);
            commando.Parameters.AddWithValue("@Tipo", miRespuesta.Tipo);
            commando.Parameters.AddWithValue("@GeneroAlerta", miRespuesta.GeneroAlerta);
            if (miRespuesta.GeneroAlerta) {
                commando.Parameters.AddWithValue("@PersonasNotificadas", miRespuesta.PersonasNotificadas);
            }


            IdRespuesta = Convert.ToInt32(commando.ExecuteScalar());
            

            conexionSQL.Close();

            return IdRespuesta;
        }

        public List<Correo> getCorreosEmpleadosSinRespuesta()
        {
            List<Correo> Correo = new List<Correo>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtieneCorreosSinRespuesta", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = commando.ExecuteReader();

            while (reader.Read())
            {
                Correo.Add(new Correo()
                {
                    CorreoElectronico = reader["Correo"].ToString().Trim(),
                    Nombre = reader["Nombre"].ToString().Trim()
                });
            }

            conexionSQL.Close();

            return Correo;
        }

        public List<DetalleInformePersona> GetInformeGeneral(RespuestaFilterDTO respuestaFilterDTO)
        {
            List<DetalleInformePersona> informe = new List<DetalleInformePersona>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ReporteGeneral", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;

            if (respuestaFilterDTO.FiltroBusqueda != null)
            {
                commando.Parameters.AddWithValue("@TextoFiltro", respuestaFilterDTO.FiltroBusqueda);
            }
            commando.Parameters.AddWithValue("@FechaInicial", respuestaFilterDTO.FechaInicial);
            commando.Parameters.AddWithValue("@FechaFinal", respuestaFilterDTO.FechaFinal);
            commando.Parameters.AddWithValue("@Sede", (respuestaFilterDTO.Sede == null)?"0" : respuestaFilterDTO.Sede);
            commando.CommandTimeout = 120;
            SqlDataReader reader = commando.ExecuteReader();

            while (reader.Read())
            {
                informe.Add(new DetalleInformePersona()
                {
                    NumeroDocumento = reader["NumeroDocumento"].ToString().Trim(),
                    Nombre = reader["Nombre"].ToString().Trim(),
                    InicioJornanda = DBNull.Value == reader["InicioJornanda"] ? (DateTime?)null : Convert.ToDateTime(reader["InicioJornanda"]),
                    FinJornada = DBNull.Value == reader["FinJornada"] ? (DateTime?)null : Convert.ToDateTime(reader["FinJornada"]),
                    IdRespuestaInicio =  DBNull.Value == reader["IdRespuestaInicio"]? 0 : Convert.ToInt32(reader["IdRespuestaInicio"]),
                    IdRespuestaFin = DBNull.Value == reader["IdRespuestaFin"] ? 0 : Convert.ToInt32(reader["IdRespuestaFin"]),
                    GeneroAlertaInicio = DBNull.Value == reader["GeneroAlertaInicio"]? false: Convert.ToBoolean(reader["GeneroAlertaInicio"]),
                    GeneroAlertaFin = DBNull.Value == reader["GeneroAlertaFin"] ? false : Convert.ToBoolean(reader["GeneroAlertaFin"]),
                    TemperaturaInicio = DBNull.Value ==  reader["TemperaturaInicio"]? "" : reader["TemperaturaInicio"].ToString().Trim(),
                    TemperaturaFin = DBNull.Value == reader["TemperaturaFin"] ? "" : reader["TemperaturaFin"].ToString().Trim(),
                });
            }

            conexionSQL.Close();

            return informe;
        }

        public string GetSedes()
        {

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerSedes", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.CommandTimeout = 120;
            SqlDataReader reader = commando.ExecuteReader();
            string options = $"<option value='0'>TODAS</option>";
            while (reader.Read())
            {
                options += $"<option value='{reader["TextoOpcion"].ToString()}'>{reader["TextoOpcion"].ToString()}</option>";
            }

            conexionSQL.Close();

            return options;
        }
    }
}
