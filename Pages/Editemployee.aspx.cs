using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
using System;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI;

namespace EmployeeManagementSystem.Pages
{
    public partial class Editemployee : System.Web.UI.Page
    {
            private readonly EmployeeRepository _repo = new EmployeeRepository();
        /// <summary>
        /// Handles the <see cref="System.Web.UI.Page.Load"/> event for the EditEmployee page.
        /// When the page is first requested (not a postback), this method initializes page data by calling <see cref="BindDepartments"/>.
        /// </summary>
        /// <param name="sender">The source of the event (typically the page instance).</param>
        /// <param name="e">An <see cref="System.EventArgs"/> instance containing event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null || Session["UserRole"] == null)
                {
                    Response.Redirect("~/Pages/Login.aspx");
                    return;
                }

                int employeeId = Convert.ToInt32(Request.QueryString["id"]);

                var employee = _repo.GetEmployeeById(employeeId);

                if (employee != null)
                {
                    BindDepartments(employee.DepartmentId);
                }
            }
        }
        
            /// <summary>
            /// Retrieves departments from the repository and binds them to the department DropDownList.
            /// </summary>
            /// <remarks>
            /// The DataTextField is set to "DepartmentName" and the DataValueField is set to "DepartmentId".
            /// After assigning the DataSource, the DropDownList is databound via <see cref="System.Web.UI.WebControls.ListControl.DataBind"/>.
            /// </remarks>
            private void BindDepartments(int employeeDepartmentId)
            {
                ddlDepartment.DataSource = _repo.GetDepartments();
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
            var employee = _repo.GetEmployeeById(Convert.ToInt32(Request.QueryString["id"]));

            ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));

            ddlDepartment.SelectedValue = employeeDepartmentId.ToString();
            txtEmpCode.Text = employee.EmployeeCode;
            txtFirstName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtEmail.Text = employee.Email;
            txtPhone.Text = employee.Phone;
        }
        /// <summary>
        /// Handles the Submit button click on the EditEmployee page.
        /// Validates the page, constructs an <see cref="Employee"/> from the form fields,
        /// updates  it via the repository, and displays a success or error message.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// On successful insert, input fields are cleared and a success message with
        /// the generated employee ID is shown. If <c>Session["UserId"]</c> is present,
        /// it is used as the CreatedBy value; otherwise a default of 1 is used.
        /// Any exceptions during the operation are caught and displayed to the user.
        /// </remarks>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int employeeId = Convert.ToInt32(Request.QueryString["id"]);

            var emp = new Employee
            {
                EmployeeId = employeeId,
                EmployeeCode = txtEmpCode.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                DepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue),
                UpdatedAt = DateTime.Now
            };

            _repo.UpdateEmployee(emp);

            lblMessage.Text = "Employee updated successfully!";
            lblMessage.CssClass = "alert alert-success d-block";
        }
        protected void btnShowNotes_Click(object sender, EventArgs e)
            {
                pnlNotes.Visible = true;
            }
    }
}