using Dapper;
using EmployeeManagementSystem.DataAccess;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EmployeeManagementSystem.Pages
{
    public class NoteRecord
    {
        public int NoteId { get; set; }
        public string NoteType { get; set; }
        public string NoteContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public partial class EmployeeDetail : Basepage
    {
        protected override string[] AllowedRoles => new[] { "Admin" };
        private int TargetEmployeeId => int.TryParse(Request.QueryString["id"], out int val) ? val : 0;
        public List<int> EmployeeId = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TargetEmployeeId <= 0)
            {
                Response.Redirect("~/Pages/EmployeeList.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEmployeeHeader();
                LoadNotes();
            }
        }

        private void LoadEmployeeHeader()
        {
            using (var conn = DBHelper.GetConnection())
            {
                string query = @"SELECT CONCAT(FirstName, ' ', LastName)
                 FROM Employees
                 WHERE EmployeeId = @Id
                 AND IsDeleted = 0
                 LIMIT 1";
                string name = conn.ExecuteScalar<string>(query, new { Id = TargetEmployeeId });
                litEmployeeName.Text = name ?? "Employee Record";
            }
        }

        internal void LoadNotes()
        {
            using (var conn = DBHelper.GetConnection())
            {
                string sql = @"
                    SELECT NoteId, NoteType, NoteContent, CreatedAt 
                    FROM EmployeeNotes 
                    WHERE EmployeeId = @EmpId 
                    ORDER BY CreatedAt DESC;";

                var items = EmployeeId != null && EmployeeId.Count > 0 ? conn.Query<NoteRecord>(sql, new { EmpId = EmployeeId[0] }).ToList() : conn.Query<NoteRecord>(sql, new { EmpId = TargetEmployeeId }).ToList();

                if (items.Count == 0)
                {
                    lblNoNotes.Visible = true;
                    rptNotesTimeline.Visible = false;
                }
                else
                {
                    if(lblNoNotes!= null)
                    {
                        lblNoNotes.Visible = false;
                        rptNotesTimeline.Visible = true;
                        rptNotesTimeline.DataSource = items;
                        rptNotesTimeline.DataBind();
                    }
  
                }
            }
        }

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            string content = txtNoteContent.Text.Trim();
            if (string.IsNullOrWhiteSpace(content)) return;

            int currentUserId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;

            using (var conn = DBHelper.GetConnection())
            {
                if (!string.IsNullOrEmpty(hfNoteId.Value))
                {
                    int noteId = Convert.ToInt32(hfNoteId.Value);

                    conn.Execute(@"
        UPDATE EmployeeNotes
        SET NoteType = @Type,
            NoteContent = @Content
        WHERE NoteId = @NoteId",
                        new
                        {
                            Type = ddlNoteType.SelectedValue,
                            Content = txtNoteContent.Text.Trim(),
                            NoteId = noteId
                        });

                    hfNoteId.Value = "";
                    btnAddNote.Text = "Save Case Note";
                }
                else
                {
                    string insertSql = @"
                    INSERT INTO EmployeeNotes (EmployeeId, CreatedByUserId, NoteType, NoteContent)
                    VALUES (@EmpId, @UserId, @Type, @Content);";

                    conn.Execute(insertSql, new
                    {
                        EmpId = TargetEmployeeId,
                        UserId = currentUserId,
                        Type = ddlNoteType.SelectedValue,
                        Content = content
                    });
                }
               
            }

            txtNoteContent.Text = string.Empty;
            lblNoteFeedback.CssClass = "alert alert-success";
            lblNoteFeedback.Text = "Note recorded successfully.";
            lblNoteFeedback.Visible = true;

            LoadNotes();
        }

        protected void rptNotesTimeline_ItemCommand(object source,
    RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditNote")
            {
                int noteId = Convert.ToInt32(e.CommandArgument);

                using (var conn = DBHelper.GetConnection())
                {
                    var note = conn.QueryFirstOrDefault<NoteRecord>(
                        @"SELECT NoteId,
                         NoteType,
                         NoteContent
                  FROM EmployeeNotes
                  WHERE NoteId=@NoteId",
                        new { NoteId = noteId });

                    if (note != null)
                    {
                        ddlNoteType.SelectedValue = note.NoteType;
                        txtNoteContent.Text = note.NoteContent;

                        hfNoteId.Value = note.NoteId.ToString();

                        btnAddNote.Text = "Update Note";
                    }
                }
            }
        }

    }
}
