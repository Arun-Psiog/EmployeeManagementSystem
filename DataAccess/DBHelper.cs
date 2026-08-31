using System.Configuration;
using System.Data;
using MySqlConnector;

namespace EmployeeManagementSystem.DataAccess
{
    public class DBHelper
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
        public static IDbConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnString);
            conn.Open();
            return conn;
        }
    }
}