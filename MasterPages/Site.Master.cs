using System;
using System.Web.Security;


namespace EmployeeManagementSystem.MasterPages
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] != null)
            {
                lblUserStatus.Text = $"{Session["UserName"]} ({Session["UserRole"]})";
                string role = Session["UserName"].ToString();

                if (role.Contains("admin"))
                {
                    liReminders.Visible = true;
                }
                else
                {
                    liReminders.Visible = false;
                }
            }
            else
            {
                btnLogout.Visible = false;
            }
         }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("/Pages/Login.aspx");
        }
        public void UpateNameAndROleOfTheUser()
        {
            lblUserStatus.Text = $"{Session["UserName"]} ({Session["UserRole"]})";
        }
    }
}