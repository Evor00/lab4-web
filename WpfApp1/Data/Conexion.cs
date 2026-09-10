using System;
using Microsoft.Data.SqlClient;

namespace WpfApp1.Data
{
    public static class Conexion
    {
        // Cadena de conexión predeterminada (compatible con SQL Server Express, LocalDB y autenticación de Windows)
        public static string CadenaConexion { get; set; } = 
            @"Server=.;Database=NeptunoDB;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=3;";

        // Indica si está en modo de demostración (si SQL Server no está disponible en la máquina)
        public static bool ModoDemostracion { get; set; } = false;

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }

        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using var conn = ObtenerConexion();
                conn.Open();
                mensaje = "¡Conexión a SQL Server exitosa!";
                ModoDemostracion = false;
                return true;
            }
            catch (Exception ex)
            {
                mensaje = $"No se pudo conectar a SQL Server: {ex.Message}";
                ModoDemostracion = true;
                return false;
            }
        }
    }
}
