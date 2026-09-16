using Dapper;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
                if (Session["UserId"] == null || Session["UserRole"] == null)
                {
                    Response.Redirect("~/Pages/Login.aspx");
                    return;
                }
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
        /// <summary>
        /// Handles the Submit button click on the AddEmployee page.
        /// Validates the page, constructs an <see cref="Employee"/> from the form fields,
        /// inserts it via the repository, and displays a success or error message.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// On successful insert, input fields are cleared and a success message with
        /// the generated employee ID is shown. If <c>Session["UserId"]</c> is present,
        /// it is used as the CreatedBy value; otherwise a default of 1 is used.
        /// Any exceptions during the operation are caught and displayed to the user.
        /// </remarks>
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


                EmployeeDetail page = new EmployeeDetail();

                Task.Delay(100);
                using (var conn = DBHelper.GetConnection())
                {
                    var list = conn.ExecuteScalar<int>(@"
                    SELECT EmployeeId
                    FROM Employees 
                    WHERE FirstName = @EmpName;", new { EmpName = emp.FirstName });
                    page.EmployeeId = new List<int> { list };
                    if (ddlNoteType != null)
                    {
                        conn.Execute(@"
                    INSERT INTO EmployeeNotes
                    (
                        EmployeeId,
                        CreatedByUserId,
                        NoteType,
                        NoteContent,
                        CreatedAt
                    )
                    VALUES
                    (
                        @EmployeeId,
                        @createdByUserId,
                        @NoteType,
                        @NoteContent,
                        NOW()
                    )",
                        new
                        {
                            EmployeeId = page.EmployeeId[0],
                            createdByUserId = 1,
                            NoteType = ddlNoteType.SelectedValue,
                            NoteContent = txtNote.Text.Trim()
                        });
                        return;
                    }

                    int currentUserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;

                    string insertSql = @"
                    INSERT INTO EmployeeNotes (EmployeeId, CreatedByUserId, NoteType, NoteContent,CreatedAt)
                    VALUES (@EmpId, @UserId, @Type, NoteContent,@CreatedAt);";

                    conn.Execute(insertSql, new
                    {
                        EmpId = page.EmployeeId,
                        UserId = currentUserId,
                        Type = "General",
                        NoteContent = DateTime.Now,
                        CreatedAt = DateTime.Now
                    });



                    page.LoadNotes();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger d-block";
                lblMessage.Visible = true;
            }

        }
        protected void btnShowNotes_Click(object sender, EventArgs e)
        {
            pnlNotes.Visible = true;
        }
    }
}