using System;
using System.Web.Security;
using Dapper;
using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.DataAccess;
namespace EmployeeManagementSystem.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Page Load event for the Login page.
        /// Ensures initial default users are created if no users exist in the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            {
                int count = conn.ExecuteScalar<int>("SELECT COUNT(1) FROM Users");
                
                if (count == 0)
                {
                    var (adminHash, adminSalt) = PasswordHelper.HashPassword("Admin@123");
                    conn.Execute("INSERT INTO Users (Username, PasswordHash, Salt, RoleId) VALUES ('admin', @Hash, @Salt, 1)", new { Hash = adminHash, Salt = adminSalt });
                    var (hrHash, hrSalt) = PasswordHelper.HashPassword("Hr@123");
                    conn.Execute("INSERT INTO Users (Username, PasswordHash, Salt, RoleId) VALUES ('hruser', @Hash, @Salt, 2)", new { Hash = hrHash, Salt = hrSalt });
                }

            }
        }

        /// <summary>
        /// Handles the login button click event. Authenticates the user, manages failed attempt counts and lockout,
        /// initializes the session on success, logs the activity, and redirects to the employee list page.
        /// </summary>
        /// <param name="sender">The source of the event (login button).</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = @"SELECT u.UserId, u.Username, u.PasswordHash, u.Salt, u.FailedAttempts, u.IsLocked, r.RoleName
                              FROM Users u
                              JOIN Roles r ON u.RoleId = r.RoleId
                              WHERE u.Username = @Username AND u.IsActive = 1";
                var user = conn.QuerySingleOrDefault<dynamic>(sql, new { Username = txtUsername.Text.Trim() });
                if (user == null || (bool)user.IsLocked)
                {
                    lblError.Text = "Account is locked or username does not exist.";
                    lblError.Visible = true;
                    return;
                }
                if (PasswordHelper.VerifyPassword(txtPassword.Text, (string)user.PasswordHash, (string)user.Salt))
                {
                    conn.Execute("UPDATE Users SET FailedAttempts = 0 WHERE UserId = @UserId", new { UserId = (int)user.UserId });
                    Session.Clear();
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.RoleName;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    ActivityLogger.Log((int)user.UserId, "LOGIN_SUCCESS", "Users", (int)user.UserId, "User logged in.");
                    Response.Redirect("~/Pages/EmployeeList.aspx");
                }
                else
                {
                    conn.Execute("UPDATE Users SET FailedAttempts = FailedAttempts + 1, IsLocked = (FailedAttempts >= 4) WHERE UserId = @UserId", new { UserId = (int)user.UserId });
                    ActivityLogger.Log(null, "LOGIN_FAILED", "Users", (int)user.UserId, $"Incorrect password for {txtUsername.Text}");
                    lblError.Text = "Invalid username or password.";
                    lblError.Visible = true;
                }
               
            }
        }
    }
}