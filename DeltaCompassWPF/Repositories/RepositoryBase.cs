using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "server=localhost;database=Delta_Compass;port=3307;user=root;password=usbw";
            //_connectionString = "server=35.192.135.152;database=Delta_Compass;port=3306;uid=root;pwd=k2Del3gTa#l4Com5rAss6e;";
            //_connectionString = $"server=35.192.135.152;database=Delta_Compass;port=3306;uid={Environment.GetEnvironmentVariable("root")};pwd={Environment.GetEnvironmentVariable("k2Del3gTa#l4Com5rAss6e")};";
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
