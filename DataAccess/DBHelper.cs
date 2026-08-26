using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagementSystem.DataAccess
{
    public class DBHelper
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
        public static IDbConnection GetConnection()
        {
            var conn = new SqlConnection(ConnString);
            conn.Open();
            return conn;
        }
    }
}