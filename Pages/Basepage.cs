using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EmployeeManagementSystem.Pages
{
    public abstract class Basepage: Page
    {
        protected  abstract string[] AllowedRoles { get; }
        protected override void OnPreInit(EventArgs e)
        {
           base.OnPreInit(e);
            if (Session["User"] == null || Session["UserRole"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx");
                return;
            }
            else
            {
                string currentUserRole = Session["UserRole"].ToString();
                if (AllowedRoles != null  && AllowedRoles.Length > 0 && !AllowedRoles.Contains(currentUserRole))
                {
                    Response.Redirect("~/Pages/AccessDenied.aspx");
                }
            }
        }
    }
}