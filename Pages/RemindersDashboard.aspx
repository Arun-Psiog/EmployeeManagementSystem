<%@ Page Title="Reminders Dashboard" Language="C#" MasterPageFile="~/MasterPages/Site.Master" 
    AutoEventWireup="true" CodeBehind="RemindersDashboard.aspx.cs" Inherits="EmployeeManagementSystem.Pages.RemindersDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <h2>Task & Follow-Up Scheduler</h2>
            <p class="text-muted small">Manage upcoming reviews, visa expirations, and compliance tasks.</p>
        </div>
    </div>

    <!-- Add Task Card -->
    <div class="card shadow-sm border-0 mb-4">
        <div class="card-header bg-primary text-white fw-bold">Schedule New Follow-Up</div>
        <div class="card-body">
            <div class="row g-2">
                <div class="col-md-4">
                    <label class="form-label small fw-bold">Select Employee</label>
                    <asp:DropDownList ID="ddlEmployees" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-5">
                    <label class="form-label small fw-bold">Task Description</label>
                    <asp:TextBox ID="txtTaskTitle" runat="server" CssClass="form-control" 
                                 placeholder="e.g., 90-Day Probation Review" />
                </div>
                <div class="col-md-3">
                    <label class="form-label small fw-bold">Due Date</label>
                    <asp:TextBox ID="txtTargetDate" runat="server" TextMode="Date" CssClass="form-control" />
                </div>
            </div>
            <div class="text-end mt-3">
                <asp:Button ID="btnCreateTask" runat="server" Text="Add Reminder" 
                            CssClass="btn btn-dark" OnClick="btnCreateTask_Click" />
            </div>
        </div>
    </div>

    <!-- Active Tasks Table -->
    <div class="card shadow-sm border-0">
        <div class="card-header bg-dark text-white fw-bold">Pending Reminders</div>
        <div class="card-body p-0">
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" 
                          CssClass="table table-hover align-middle mb-0" 
                          OnRowCommand="gvTasks_RowCommand" 
                          EmptyDataText="No active tasks in your queue.">
                <Columns>
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee" />
                    <asp:BoundField DataField="Title" HeaderText="Task Summary" />
                    <asp:TemplateField HeaderText="Due Date">
                        <ItemTemplate>
                            <span class='<%# (bool)Eval("IsOverdue") ? "badge bg-danger" : "badge bg-secondary" %>'>
                                <%# Eval("DueDate", "{0:MMM dd, yyyy}") %>
                                <%# (bool)Eval("IsOverdue") ? " (Overdue)" : "" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-3">
                        <ItemTemplate>
                            <asp:Button ID="btnComplete" runat="server" CommandName="ResolveTask" 
                                        CommandArgument='<%# Eval("ReminderId") %>' 
                                        Text="Mark Complete" CssClass="btn btn-sm btn-outline-success" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>