<%@ Page Title="Employee List" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="EmployeeManagementSystem.Pages.EmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h3>Employee Directory</h3>
        <a href="AddEmployee.aspx" ID="addemployee" runat="server" class="btn btn-success btn-sm">+ Add New Employee</a>
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
                  <a href='<%# "EmployeeDetail.aspx?id=" +Eval("EmployeeId") %>' class='btn btn-sm btn-info text-white'><i class="bi bi-file-earthmark-text"></i>Notes</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="d-flex justify-content-between align-items-center mt-3">
        <asp:Button ID="btnPrev" runat="server" Text="&laquo; Previous" CssClass="btn btn-outline-secondary" OnClick="btnPrev_Click" />
        <asp:Label ID="lblPageStatus" runat="server" CssClass="fw-bold"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Text="Next &raquo;" CssClass="btn btn-outline-secondary" OnClick="btnNext_Click" />
    </div>
</asp:Content>
