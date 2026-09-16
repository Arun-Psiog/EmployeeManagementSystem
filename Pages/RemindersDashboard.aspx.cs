using Dapper;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem.Pages
{
    public class EmployeeGridDtoRemaidnder
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class TaskItemDto
    {
        public int ReminderId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsOverdue => DueDate.Date < DateTime.UtcNow.Date;
    }


    public partial class RemindersDashboard : Basepage
    {

        protected override string[] AllowedRoles => new[] { "Admin" };
        private int ActiveUserId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadEmployeeSelector();
                BindRemindersGrid();
            }
        }

        private void LoadEmployeeSelector()
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = "SELECT EmployeeId, FirstName FROM Employees WHERE IsDeleted = 0 ORDER BY FirstName ASC;";
                var employees = conn.Query<EmployeeGridDtoRemaidnder>(sql).ToList();

                ddlEmployees.DataSource = employees;
                ddlEmployees.DataTextField = "FirstName";
                ddlEmployees.DataValueField = "EmployeeId";
                ddlEmployees.DataBind();
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
                    INNER JOIN Employees e ON r.EmployeeId = e.EmployeeId
                    WHERE r.AssignedToUserId = @UserId AND r.IsResolved = 0
                    ORDER BY r.DueDate ASC;";

                var data = conn.Query<TaskItemDto>(sql, new { UserId = ActiveUserId }).ToList();
                gvTasks.DataSource = data;
                gvTasks.DataBind();
            }
        }

        protected void btnCreateTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskTitle.Text) || !DateTime.TryParse(txtTargetDate.Text, out DateTime due))
            {
                return;
            }

            int targetEmpId = Convert.ToInt32(ddlEmployees.SelectedValue);

            using (var conn = DBHelper.GetConnection())
            {
                string insertSql = @"
                    INSERT INTO EmployeeReminders (EmployeeId, AssignedToUserId, Title, DueDate)
                    VALUES (@EmpId, @UserId, @Title, @Due);";

                conn.Execute(insertSql, new
                {
                    EmpId = targetEmpId,
                    UserId = ActiveUserId,
                    Title = txtTaskTitle.Text.Trim(),
                    Due = due
                });
            }

            txtTaskTitle.Text = string.Empty;
            txtTargetDate.Text = string.Empty;
            BindRemindersGrid();
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResolveTask")
            {
                int reminderId = Convert.ToInt32(e.CommandArgument);

                using (var conn = DBHelper.GetConnection())
                {
                    string resolveSql = @"
                        UPDATE EmployeeReminders 
                        SET IsResolved = 1, ResolvedAt = NOW() 
                        WHERE ReminderId = @Id;";

                    conn.Execute(resolveSql, new { Id = reminderId });
                }

                BindRemindersGrid();
            }
        }
    }
}
