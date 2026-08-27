using System;
using System.Web.UI.WebControls;
using EmployeeManagementSystem.DataAccess;
namespace EmployeeManagementSystem.Pages
{
    public partial class EmployeeList : System.Web.UI.Page
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        private const int PageSize = 5;
        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDepartments();
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
        private void LoadData()
        {
            int? deptId = Convert.ToInt32(ddlFilterDept.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterDept.SelectedValue) : (int?)null;
            var (records, totalCount) = _repo.GetPagedEmployees(txtSearch.Text.Trim(), deptId, CurrentPage, PageSize);
            gvEmployees.DataSource = records;
            gvEmployees.DataBind();
            int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
            lblPageStatus.Text = $"Page {CurrentPage} of {(totalPages == 0 ? 1 : totalPages)} (Total: {totalCount})";
            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            LoadData();
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadData();
        }
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
    }
}