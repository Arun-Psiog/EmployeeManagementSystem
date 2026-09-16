<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomFieldsControl.ascx.cs" 
    Inherits="EmployeeManagementSystem.Controls.CustomFieldsControl" %>

<div class="card mb-3 shadow-sm border-0">
    <div class="card-header bg-secondary text-white fw-bold">Dynamic Metadata Fields</div>
    <div class="card-body">
        <asp:Repeater ID="rptFields" runat="server">
            <ItemTemplate>
                <div class="mb-3">
                    <asp:HiddenField ID="hfFieldId" runat="server" Value='<%# Eval("FieldId") %>' />
                    <label class="form-label small fw-bold text-dark"><%# Eval("FieldName") %></label>
                    <asp:TextBox ID="txtFieldValue" runat="server" Text='<%# Eval("FieldValue") %>' 
                                 CssClass="form-control form-control-sm" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblEmptyNotice" runat="server" Text="No custom fields are registered in the system." 
                   Visible="false" CssClass="text-muted small fst-italic" />
    </div>
</div>
