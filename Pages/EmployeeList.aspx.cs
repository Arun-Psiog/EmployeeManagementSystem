using Dapper;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.MasterPages;
using System;
using System.Web.UI.WebControls;
using System.Linq;

namespace EmployeeManagementSystem.Pages
{
    public partial class EmployeeList : System.Web.UI.Page
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        protected string[] AllowedRoles => new[] { "Admin" };
        private int ActiveUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;
        private const int PageSize = 5;
        /// <summary>
        /// Gets or sets the current page number for paging operations.
        /// </summary>
        /// <remarks>
        /// The value is persisted in the page's <see cref="ViewState"/> under the "CurrentPage" key.
        /// If no value is present in <see cref="ViewState"/>, the getter returns 1.
        /// </remarks>
        /// <value>The current page index (1-based).</value>
        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null || Session["UserRole"] == null)
                {
                    Response.Redirect("~/Pages/Login.aspx");
                    return;
                }
                string currentUserRole = Session["UserRole"].ToString();
                if (AllowedRoles != null && AllowedRoles.Length > 0 && !AllowedRoles.Contains(currentUserRole))
                {
                    gvTasks.Visible = false;
                    //hideorshow.Visible = false;
                }
                BindDepartments();
                BindRemindersGrid();
                LoadData();
            }
        }
        private void BindDepartments()
        {
            ddlFilterDept.DataSource = _repo.GetDepartments();
            ddlFilterDept.DataTextField = "DepartmentName";
            ddlFilterDept.DataValueField = "DepartmentId";
            ddlFilterDept.DataBind();
            ddlFilterDept.Items.Insert(0, new ListItem("-- All Departments --", "0"));
        }
        /// <summary>
        /// Loads and binds the employee data to the GridView using the current filter, search text and paging settings.
        /// </summary>
        /// <remarks>
        /// - Reads the selected department filter from <see cref="ddlFilterDept"/>; a value of 0 is treated as no filter.
        /// - Calls the repository <c>GetPagedEmployees</c> to retrieve the current page of records and the total count.
        /// - Binds the returned records to <see cref="gvEmployees"/> and updates paging UI elements:
        ///   <see cref="lblPageStatus"/>, <see cref="btnPrev"/>, and <see cref="btnNext"/>.
        /// - Hides the delete LinkButton in each data row for users who are not allowed to delete (based on <c>Session["UserId"]</c> or <c>Session["UserRole"]</c>).
        /// </remarks>
        private void LoadData()
        {
            int? deptId = Convert.ToInt32(ddlFilterDept.SelectedValue) > 0
                ? Convert.ToInt32(ddlFilterDept.SelectedValue)
                : (int?)null;

            var (records, totalCount) = _repo.GetPagedEmployees(
                txtSearch.Text.Trim(),
                deptId,
                CurrentPage,
                PageSize);

            gvEmployees.DataSource = records;
            gvEmployees.DataBind();

            int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            lblPageStatus.Text =
                $"Page {CurrentPage} of {(totalPages == 0 ? 1 : totalPages)} (Total: {totalCount})";

            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;

            if (Session["UserId"]?.ToString() != "1" &&
                Session["UserId"]?.ToString() != "2")
            {
                addemployee.Visible = false;
                foreach (GridViewRow row in gvEmployees.Rows)
                {
                    LinkButton btnDelete =
                        (LinkButton)row.FindControl("btnDelete");

                    if (btnDelete != null)
                    {
                        btnDelete.Visible = false;
                    }
                }
            }

            Site masterPage = (Site)this.Master;
            masterPage.UpateNameAndROleOfTheUser();
        }

        /// <summary>
        /// Handles the Filter button click event. Resets the current page to the first page
        /// and reloads the employee data using the current filter criteria.
        /// </summary>
        /// <param name="sender">The source of the event (Filter button).</param>
        /// <param name="e">Event arguments.</param>
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        /// <summary>
        /// Handles the Previous page button click event. Decrements the current page index
        /// and reloads the employee data for the new page.
        /// </summary>
        /// <param name="sender">The source of the event (Previous button).</param>
        /// <param name="e">Event arguments.</param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            LoadData();
        }

        /// <summary>
        /// Handles the Next page button click event. Increments the current page index
        /// and reloads the employee data for the new page.
        /// </summary>
        /// <param name="sender">The source of the event (Next button).</param>
        /// <param name="e">Event arguments.</param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadData();
        }

        /// <summary>
        /// Handles row command events raised by the employee GridView. Supports soft-deleting
        /// an employee when the command name is "SoftDelete".
        /// </summary>
        /// <param name="sender">The source of the event (GridView).</param>
        /// <param name="e">GridView command event arguments containing the command name and argument.</param>
        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SoftDelete")
            {
                int empId = Convert.ToInt32(e.CommandArgument);
                int userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;
                _repo.SoftDeleteEmployee(empId, userId);
                LoadData();
            }
        }

        /// <summary>
        /// Handles the RowDataBound event for the employee GridView. Controls visibility
        /// of the delete LinkButton based on the current user's role stored in session.
        /// </summary>
        /// <param name="sender">The source of the event (GridView).</param>
        /// <param name="e">GridView row event arguments containing the row being bound.</param>
        protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = e.Row.FindControl("btnDelete") as LinkButton;
                if (deleteBtn != null && Session["UserRole"] != null)
                {
                    deleteBtn.Visible = Session["UserRole"].ToString() == "Admin";
                }
            }
        }

        private void BindRemindersGrid()
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = @"
                SELECT
                    r.ReminderId,
                    e.FirstName AS EmployeeName,
                    r.Title,
                    r.DueDate
                FROM EmployeeReminders r
                INNER JOIN Employees e
                    ON r.EmployeeId = e.EmployeeId
                WHERE r.AssignedToUserId = @UserId
                    AND r.IsResolved = 0
                    AND DATE(r.DueDate) <= CURDATE()
                ORDER BY r.DueDate ASC;";

                var data = conn.Query<TaskItemDto>(sql, new { UserId = ActiveUserId }).ToList();
                gvTasks.DataSource = data;
                gvTasks.DataBind();
            }
        }

    }
}