using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DeltaCompassWPF.Database
{
    class Conexao
    {
        private static string host = "localhost";
        private static string port = "3307";
        private static string user = "root";
        private static string password = "usbw";
        private static string dbname = "Delta_Compass";
        private static MySqlConnection connection;

        public Conexao()
        {
            try
            {
                connection = new MySqlConnection($"server={host};database={dbname};port={port};user={user};password={password}");
                connection.Open();
            } catch (Exception)
            {
                throw;
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
