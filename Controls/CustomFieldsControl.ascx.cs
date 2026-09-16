using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using EmployeeManagementSystem.DataAccess;

namespace EmployeeManagementSystem.Controls
{
    public class DynamicFieldDto
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }

    public partial class CustomFieldsControl : UserControl
    {
        public int TargetEmployeeId
        {
            get => ViewState["CurrentEmpId"] != null ? (int)ViewState["CurrentEmpId"] : 0;
            set => ViewState["CurrentEmpId"] = value;
        }

        public void LoadValues(int empId)
        {
            this.TargetEmployeeId = empId;

            using (var conn = DBHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        d.FieldId, 
                        d.FieldName, 
                        v.FieldValue 
                    FROM CustomFieldDefinitions d
                    LEFT JOIN EmployeeCustomValues v 
                        ON d.FieldId = v.FieldId AND v.EmployeeId = @EmpId
                    ORDER BY d.FieldName ASC;";

                var list = conn.Query<DynamicFieldDto>(sql, new { EmpId = this.TargetEmployeeId }).ToList();

                if (list.Count == 0)
                {
                    lblEmptyNotice.Visible = true;
                    rptFields.Visible = false;
                }
                else
                {
                    lblEmptyNotice.Visible = false;
                    rptFields.Visible = true;
                    rptFields.DataSource = list;
                    rptFields.DataBind();
                }
            }
        }

        public void SaveValues()
        {
            if (this.TargetEmployeeId <= 0) return;

            using (var conn = DBHelper.GetConnection())
            {
                foreach (RepeaterItem item in rptFields.Items)
                {
                    var hfId = (HiddenField)item.FindControl("hfFieldId");
                    var txtVal = (TextBox)item.FindControl("txtFieldValue");

                    if (hfId != null && txtVal != null)
                    {
                        int fieldId = Convert.ToInt32(hfId.Value);
                        string val = txtVal.Text.Trim();

                        string upsertSql = @"
                            INSERT INTO EmployeeCustomValues (EmployeeId, FieldId, FieldValue)
                            VALUES (@EmpId, @FId, @Val)
                            ON DUPLICATE KEY UPDATE FieldValue = @Val;";

                        conn.Execute(upsertSql, new
                        {
                            EmpId = this.TargetEmployeeId,
                            FId = fieldId,
                            Val = val
                        });
                    }
                }
            }
        }
    }
}


