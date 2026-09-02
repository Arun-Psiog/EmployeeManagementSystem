using System;
using System.IO;
using System.Web.UI.WebControls;
using Dapper;
using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.DataAccess;
namespace EmployeeManagementSystem.Pages
{
    public partial class BulkEmployeeUpload : Basepage
    {
        protected override string[] AllowedRoles => new[] { "Admin", "HR" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadGrid();
        }
        private void LoadGrid()
        {
            using (var conn = DBHelper.GetConnection())
            {
                gvJobs.DataSource = conn.Query("SELECT * FROM BulkUploadJobs ORDER BY JobId DESC");
                gvJobs.DataBind();
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!fileUploadCsv.HasFile) return;
            string uploadFolder = Server.MapPath("~/App_Data/Uploads/");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);
            string savedName = Guid.NewGuid() + "_" + fileUploadCsv.FileName;
            fileUploadCsv.SaveAs(Path.Combine(uploadFolder, savedName));
            using (var conn = DBHelper.GetConnection())
            {
                string sql = "INSERT INTO BulkUploadJobs (FileName, Status, CreatedBy, CreatedAt) VALUES (@FileName, 'Queued', @UserId, NOW())";
                conn.Execute(sql, new { FileName = savedName, UserId = Convert.ToInt32(Session["UserId"]) });
            }
            lblStatus.Text = "File uploaded successfully and placed into processing queue.";
            lblStatus.Visible = true;
            LoadGrid();
        }
        protected void gvJobs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RollbackJob")
            {
                if (Session["UserRole"].ToString() != "Admin")
                {
                    lblStatus.Text = "Unauthorized: Only Admins can rollback jobs.";
                    lblStatus.CssClass = "alert alert-danger d-block";
                    lblStatus.Visible = true;
                    return;
                }
                int jobId = Convert.ToInt32(e.CommandArgument);
                BackgroundWorkerService.RollbackBatch(jobId, Convert.ToInt32(Session["UserId"]));
                LoadGrid();
            }
        }
    }
}