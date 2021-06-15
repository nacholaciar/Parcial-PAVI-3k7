using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialPav.AccesoADatos
{
    public class Acceso
    {
        public static int NumeroEquipo()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand comando = new SqlCommand();

                string consulta = "select MAX(Id) from Equipos";

                comando.Parameters.Clear();
                comando.CommandType = CommandType.Text;
                comando.CommandText = consulta;

                conexion.Open();
                comando.Connection = conexion;

                int resultado = (int)comando.ExecuteScalar();
                return resultado;

            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conexion.Close();
            }
        } // SQL

        public static DataTable cargarComboCategorias()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand comando = new SqlCommand();

                string consulta = "select * from Categorias";

                comando.Parameters.Clear();
                comando.CommandType = CommandType.Text;
                comando.CommandText = consulta;

                conexion.Open();
                comando.Connection = conexion;

                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(tabla);

                return tabla;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conexion.Close();
            }
        } // SQL

        public static DataTable cargarComboPosiciones()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand comando = new SqlCommand();

                string consulta = "select * from Posiciones";

                comando.Parameters.Clear();
                comando.CommandType = CommandType.Text;
                comando.CommandText = consulta;

                conexion.Open();
                comando.Connection = conexion;

                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(tabla);

                return tabla;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conexion.Close();
            }
        } // SQL

        public static DataTable ObtenerNombreJugador(string nroJugador)
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection conexion = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand comando = new SqlCommand();

                string consulta = "SELECT * from Jugadores where Id = @nrojugador";

                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@nrojugador", nroJugador);
                comando.CommandType = CommandType.Text;
                comando.CommandText = consulta;

                conexion.Open(); 
                comando.Connection = conexion;

                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(tabla);

                return tabla;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            finally
            {
                conexion.Close();
            }

        }

        public static bool AltaNuevoEquipo(int nroNuevoEquipo, string nombreEquipo, string fecha, List<int> listaIdJugadores)
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlTransaction objTransaccion = null;
            SqlConnection conexion = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand comando = new SqlCommand();

                string consulta = "insert into Equipos values(@nombrequipo, @fecha)";

                comando.Parameters.Clear(); 
                comando.Parameters.AddWithValue("@nombrequipo", nombreEquipo);
                comando.Parameters.AddWithValue("@fecha", fecha);
                comando.CommandType = CommandType.Text;
                comando.CommandText = consulta;

                conexion.Open();
                objTransaccion = conexion.BeginTransaction("altaEquipo");
                comando.Transaction = objTransaccion;
                comando.Connection = conexion;

                comando.ExecuteNonQuery();
                foreach (var idjugador in listaIdJugadores)
                {
                    string consultaJugadoresPorEquipo = "insert into JugadoresPorEquipos values(@IdJugador, @IdEquipo, @FechaAsignacion)";
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@IdJugador", idjugador);
                    comando.Parameters.AddWithValue("@IdEquipo", nroNuevoEquipo);
                    comando.Parameters.AddWithValue("@FechaAsignacion", DateTime.Now);

                    comando.CommandText = consultaJugadoresPorEquipo;
                    comando.ExecuteNonQuery();
                }

                objTransaccion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                objTransaccion.Rollback();
                return false;
            }
            finally
            {
                conexion.Close();
            }

        }
    }
}
