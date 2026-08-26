using System;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
namespace EmployeeManagementSystem.Pages
{
    public partial class AddEmployee : System.Web.UI.Page
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartments();
            }
        }
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