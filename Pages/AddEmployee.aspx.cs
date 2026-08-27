using System;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
namespace EmployeeManagementSystem.Pages
{
    public partial class AddEmployee : System.Web.UI.Page
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        /// <summary>
        /// Handles the <see cref="System.Web.UI.Page.Load"/> event for the AddEmployee page.
        /// When the page is first requested (not a postback), this method initializes page data by calling <see cref="BindDepartments"/>.
        /// </summary>
        /// <param name="sender">The source of the event (typically the page instance).</param>
        /// <param name="e">An <see cref="System.EventArgs"/> instance containing event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartments();
            }
        }
        /// <summary>
        /// Retrieves departments from the repository and binds them to the department DropDownList.
        /// </summary>
        /// <remarks>
        /// The DataTextField is set to "DepartmentName" and the DataValueField is set to "DepartmentId".
        /// After assigning the DataSource, the DropDownList is databound via <see cref="System.Web.UI.WebControls.ListControl.DataBind"/>.
        /// </remarks>
        private void BindDepartments()
        {
            ddlDepartment.DataSource = _repo.GetDepartments();
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                var emp = new Employee
                {
                    EmployeeCode = txtEmpCode.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    DepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue),
                    CreatedBy = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                int newId = _repo.InsertEmployee(emp);
                lblMessage.Text = $"Employee saved successfully! Generated ID: {newId}";
                lblMessage.CssClass = "alert alert-success d-block";
                lblMessage.Visible = true;
                txtEmpCode.Text = txtFirstName.Text = txtLastName.Text = txtEmail.Text = txtPhone.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger d-block";
                lblMessage.Visible = true;
            }
        }
    }
}