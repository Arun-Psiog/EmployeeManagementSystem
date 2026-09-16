<%@ Page Title="Edit Employee" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditEmployee.aspx.cs" Inherits="EmployeeManagementSystem.Pages.Editemployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card p-4 mx-auto shadow-sm" style="max-width: 600px;">
        <h3 class="mb-3">Edit Employee</h3>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info d-block" Visible="false"></asp:Label>
        <div class="mb-3">
            <label class="form-label">Employee Code</label>
            <asp:TextBox ID="txtEmpCode" runat="server" CssClass="form-control" placeholder="e.g. EMP001"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Code is required." ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." ForeColor="Red" Display="Dynamic" />
            </div>
            <div class="col-md-6 mb-3">
                <label class="form-label">Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." ForeColor="Red" Display="Dynamic" />
            </div>
        </div>
        <div class="mb-3">
            <label class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@company.com"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Invalid email format." ForeColor="Red" Display="Dynamic" />
        </div>
        <div class="mb-3">
            <label class="form-label">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Department</label>
            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select"></asp:DropDownList>
        </div>
        <asp:Button ID="btnShowNotes"
            runat="server"
            Text="+ Add Note"
            CssClass="btn btn-outline-secondary btn-sm"
            OnClick="btnShowNotes_Click" />

        <asp:Panel ID="pnlNotes"
            runat="server"
            Visible="false">

            <div class="mt-3">
                <label>Note Type</label>
                <asp:DropDownList ID="ddlNoteType"
                    runat="server"
                    CssClass="form-select">
                    <asp:ListItem Text="Call" />
                    <asp:ListItem Text="Email" />
                    <asp:ListItem Text="Meeting" />
                    <asp:ListItem Text="Follow Up" />
                </asp:DropDownList>
            </div>

            <div class="mt-2">
                <label>Note</label>
                <asp:TextBox ID="txtNote"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="4"
                    CssClass="form-control">
                </asp:TextBox>
            </div>

        </asp:Panel>
        <asp:Button ID="btnSubmit" runat="server" Text="Save Employee" CssClass="btn btn-primary w-100 mt-3" OnClick="btnUpdate_Click" />
    </div>
</asp:Content>
