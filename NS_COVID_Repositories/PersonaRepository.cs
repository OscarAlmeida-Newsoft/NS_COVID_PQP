using NS_COVID_Entities;
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
    public class PersonaRepository
    {
        private SqlConnection conexionSQL;

        public PersonaRepository() {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            conexionSQL = new SqlConnection(connectionString);
        }

        public Persona getPersonaByCedula(string cedula)
        {
            Persona laPersona = null;

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerPersonaPorDocumento", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@NumeroDocumento", cedula);

            SqlDataReader reader = commando.ExecuteReader();


            if (reader.Read())
            {
                laPersona = new Persona() {
                    Id = Convert.ToInt32(reader["Id"]),
                    NumeroDocumento = reader["NumeroDocumento"].ToString().Trim(),
                    Nombres = reader["Nombres"].ToString().Trim(),
                    Apellidos = reader["Apellidos"].ToString().Trim(),
                    Vinculo = reader["Vinculo"].ToString().Trim(),
                    NumeroCelular = reader["NumeroCelular"].ToString().Trim(),
                    GeneroAlerta = Convert.ToBoolean(reader["GeneroAlerta"]),
                    FechaHoraCreacion = reader["FechaHoraCreacion"].ToString()
                };

                
            }

            conexionSQL.Close();

            

            return laPersona;

        }

        public int createPersona(Persona laPersona)
        {
            conexionSQL.Open();
            int idPersona = 0;

            SqlCommand commando = new SqlCommand("COVID_CreaPersona", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;

            commando.Parameters.AddWithValue("@TipoDocumento", laPersona.TipoDocumento);
            commando.Parameters.AddWithValue("@NumeroDocumento", laPersona.NumeroDocumento);
            commando.Parameters.AddWithValue("@Nombres", laPersona.Nombres);
            commando.Parameters.AddWithValue("@Apellidos", laPersona.Apellidos);
            commando.Parameters.AddWithValue("@Sexo", laPersona.Sexo);
            commando.Parameters.AddWithValue("@FechaNacimiento", laPersona.FechaNacimiento);
            commando.Parameters.AddWithValue("@NumeroCelular", laPersona.NumeroCelular);
            commando.Parameters.AddWithValue("@CorreoElectronico", laPersona.CorreoElectronico);
            commando.Parameters.AddWithValue("@Vinculo", laPersona.Vinculo);
            commando.Parameters.AddWithValue("@GeneroAlerta", laPersona.GeneroAlerta);

            //TODO: Agregar parametro de GeneroAlerta

            if (laPersona.Empresa != null)
            {
                commando.Parameters.AddWithValue("@Empresa", laPersona.Empresa);
            }
            if (laPersona.Placa != null)
            {
                commando.Parameters.AddWithValue("@Placa", laPersona.Placa);
            }
            commando.Parameters.AddWithValue("@AceptoTerminosUso", laPersona.AceptaTerminosUso);

            //commando.ExecuteNonQuery();

            //TODO: Obtener IdPersona del Procedimiento SELECT SCOPE_IDENTITY()
            idPersona = Convert.ToInt32(commando.ExecuteScalar());

            conexionSQL.Close();

            return idPersona;
        }

        public Persona getPersonaById(int idPersona)
        {
            Persona laPersona = null;

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerPersonaPorId", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@Id", idPersona);

            SqlDataReader reader = commando.ExecuteReader();


            if (reader.Read())
            {
                laPersona = new Persona()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    NumeroDocumento = reader["NumeroDocumento"].ToString().Trim(),
                    Nombres = reader["Nombres"].ToString().Trim(),
                    Apellidos = reader["Apellidos"].ToString().Trim(),
                    FechaHoraCreacion = $"{DateTime.Now.ToShortDateString()} a las horas {DateTime.Now.ToShortTimeString()}",
                    Vinculo = reader["Vinculo"].ToString().Trim(),
                    NumeroCelular = reader["NumeroCelular"].ToString().Trim(),
                    GeneroAlerta = Convert.ToBoolean(reader["GeneroAlerta"])
                };


            }

            conexionSQL.Close();



            return laPersona;
        }

        public List<Accesos> GetPermissionsByUsers(string userName)
        {
            List<Accesos> detalles = new List<Accesos>();

            conexionSQL.Open();

            SqlCommand commando = new SqlCommand("COVID_ObtenerUsuariosByUser", conexionSQL);
            commando.CommandType = CommandType.StoredProcedure;
            commando.Parameters.AddWithValue("@userName", userName);

            SqlDataReader reader = commando.ExecuteReader();


            while (reader.Read())
            {

                Accesos det = new Accesos();
                det.Rutas = reader["Ruta"].ToString();
                det.Nombre = reader["Nombre"].ToString();

                detalles.Add(det);
            }

            conexionSQL.Close();

            return detalles;
        }
    }
}
