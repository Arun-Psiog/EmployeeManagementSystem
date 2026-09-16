<%@ Page Title="Employee List" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="EmployeeManagementSystem.Pages.EmployeeList" %>

<%@ Register Src="~/Controls/CustomFieldsControl.ascx" TagPrefix="uc1" TagName="CustomFieldsControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            background: linear-gradient(135deg, #f5f7fa, #dfe9f3);
            min-height: 100vh;
        }
    </style>
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h3>Employee Directory</h3>
        <a href="AddEmployee.aspx" id="addemployee" runat="server" class="btn btn-success btn-sm">+ Add New Employee</a>
    </div>
    <div class="card p-3 mb-3 bg-light">
        <div class="row g-2">
            <div class="col-md-5">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search by code or name..."></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlFilterDept" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary w-100" OnClick="btnFilter_Click" />
            </div>
        </div>
    </div>
    <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" OnRowCommand="gvEmployees_RowCommand" OnRowDataBound="gvEmployees_RowDataBound">
        <Columns>
            <asp:BoundField DataField="EmployeeCode" HeaderText="Code" />
            <asp:BoundField DataField="FullName" HeaderText="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Joined" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="SoftDelete" CommandArgument='<%# Eval("EmployeeId") %>' CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Confirm soft delete?');">Delete</asp:LinkButton>
                    <a href='<%# "EmployeeDetail.aspx?id=" +Eval("EmployeeId") %>' class='btn btn-sm btn-info text-white'><i class="bi bi-file-earthmark-text"></i>Notes</a>                   <a href='<%# "Editemployee.aspx?id=" + Eval("EmployeeId") %>' class="btn-warning"><i class="bi bi-pencil-square"></i>✏️ Edit</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="d-flex justify-content-between align-items-center mt-3">
        <asp:Button ID="btnPrev" runat="server" Text="&laquo; Previous" CssClass="btn btn-outline-secondary" OnClick="btnPrev_Click" />
        <asp:Label ID="lblPageStatus" runat="server" CssClass="fw-bold"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Text="Next &raquo;" CssClass="btn btn-outline-secondary" OnClick="btnNext_Click" />
    </div>
    <div class="card mt-4 border-0 shadow-sm">
        <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
            <span class="fw-bold">Manage Employee Custom Fields</span>
        </div>
        <div class="card-body">
            <asp:Label ID="lblCustomFieldMsg" runat="server" CssClass="alert d-block mb-3" Visible="false"></asp:Label>
            <!-- 1. Select employee to load their fields -->
            <div class="row g-2 mb-3 align-items-end">
                <div class="col-md-5">
                    <label class="form-label small fw-bold">Select Employee to View/Edit Metadata:</label>
                    <asp:DropDownList ID="ddlFieldEmployee" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnLoadEmployeeFields" runat="server" Text="Load Custom Fields"
                        CssClass="btn btn-outline-primary" OnClick="btnLoadEmployeeFields_Click" />
                </div>
            </div>
            <!-- 2. The User Control -->
            <asp:Panel ID="pnlFieldsContainer" runat="server" Visible="false">
                <uc1:CustomFieldsControl ID="myCustomFields" runat="server" />
                <!-- 3. Save Button -->
                <div class="mt-2 text-end">
                    <asp:Button ID="btnSaveFields" runat="server" Text="Save Custom Fields"
                        CssClass="btn btn-success" OnClick="btnSaveFields_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
    <%--Todays Remainder section--%>
    <div class="mt-4">
        <div id="hideorshow" class="card shadow-sm border-0">
            <div class="card-header bg-dark text-white fw-bold">Today's Reminders</div>
            <div class="card-body p-0">
                <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover align-middle mb-0"
                    EmptyDataText="No active tasks in your queue.">
                    <Columns>
                        <asp:BoundField DataField="EmployeeName" HeaderText="Employee" />
                        <asp:BoundField DataField="Title" HeaderText="Task Summary" />
                        <asp:TemplateField HeaderText="Due Date">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("IsOverdue") ? "badge bg-danger" : "badge bg-secondary" %>'><%# Eval("DueDate", "{0:MMM dd, yyyy}") %>                                    <%# (bool)Eval("IsOverdue") ? " (Overdue)" : "" %>                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <%--End Of Today' Remaninders secion--%>
</asp:Content>
