using System;
using System.Collections.Generic;
using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Entities;
namespace EmployeeManagementSystem.Pages
{
    public partial class ReportBuilder : Basepage
    {
        /// <summary>
        /// Roles allowed to access this page.
        /// </summary>
        protected override string[] AllowedRoles => new[] { "Admin", "HR" };

        /// <summary>
        /// Repository used to retrieve employee and department data.
        /// </summary>
        private readonly EmployeeRepository _repo = new EmployeeRepository();

        /// <summary>
        /// Handles the Page Load event. Initializes department dropdown on first load.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlDept.DataSource = _repo.GetDepartments();
                ddlDept.DataTextField = "DepartmentName";
                ddlDept.DataValueField = "DepartmentId";
                ddlDept.DataBind();
                ddlDept.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Departments --", "0"));
            }
        }

        /// <summary>
        /// Fetches employee data to be used by the report preview and exporters.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="EmployeeGridDto"/> representing the employees.
        /// If a department is selected in the department dropdown, results are filtered by that department.
        /// </returns>
        private IEnumerable<EmployeeGridDto> FetchData()
        {
            int? deptId = Convert.ToInt32(ddlDept.SelectedValue) > 0 ? Convert.ToInt32(ddlDept.SelectedValue) : (int?)null;
            var (records, _) = _repo.GetPagedEmployees(null, deptId, 1, 1000);
            return records;
        }

        /// <summary>
        /// Handles the Preview button click. Binds fetched data to the preview grid.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            gvReportPreview.DataSource = FetchData();
            gvReportPreview.DataBind();
        }

        /// <summary>
        /// Handles the CSV export button click. Exports the current data to CSV and initiates a download.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        protected void btnCsv_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportExporter.ExportToCsv(FetchData());
            DownloadFile(bytes, "text/csv", "EmployeeReport.csv");
        }

        /// <summary>
        /// Handles the PDF export button click. Exports the current data to PDF and initiates a download.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportExporter.ExportToPdf(FetchData());
            DownloadFile(bytes, "application/pdf", "EmployeeReport.pdf");
        }

        /// <summary>
        /// Sends a file to the client as an HTTP attachment.
        /// </summary>
        /// <param name="fileBytes">The file content as a byte array.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="fileName">The filename presented to the client.</param>
        private void DownloadFile(byte[] fileBytes, string contentType, string fileName)
        {
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
            Response.BinaryWrite(fileBytes);
            Response.End();

        }

    }

}
