using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.DataAccess;
using System;
using Dapper;


namespace EmployeeManagementSystem.Pages
{
    public partial class CreatePassword : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Create button click. If there is at least one user in the Users table,
        /// creates a new admin user using the username from <c>TextBox2</c> and the password from <c>txtnewPassword</c>.
        /// The password is hashed (with a salt) using <see cref="PasswordHelper.HashPassword"/> and the new user
        /// is inserted into the database. The username is also stored in <c>Session["UserName"]</c>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            {
                int count = conn.ExecuteScalar<int>("SELECT COUNT(1) FROM Users");
                Session["UserName"] = TextBox2.Text;
                if (count != 0)
                {
                    var (adminHash, adminSalt) = PasswordHelper.HashPassword(txtnewPassword.Text);
                    var seasionName = Session["UserName"].ToString();
                    conn.Execute("INSERT INTO Users (Username, PasswordHash, Salt, RoleId) VALUES (@Username, @Hash, @Salt, 1)", new { Username = seasionName, Hash = adminHash, Salt = adminSalt });
                }
            }
        }
    }
}