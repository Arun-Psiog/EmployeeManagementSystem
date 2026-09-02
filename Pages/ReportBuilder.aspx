<%@ Page Title="Report Builder" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeBehind="ReportBuilder.aspx.cs" Inherits="EmployeeManagementSystem.Pages.ReportBuilder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Filter-Based Report Builder</h3>
    <div class="row p-3 mb-4 bg-light">
        <div class="row g-3">
            <div class="col-md-4">
                <label class="form-label">Department:</label>
                <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <div class="col-md-4 align-self-end">
               <asp:Button ID="btnPreview" runat="server" Text="Preview Report" CssClass="btn btn-secondary w-100" OnClick="btnPreview_Click" />
            </div>
        </div>
    </div>
    <asp:GridView ID="gvReportPreview" runat="server" CssClass="table table-bordered mb-3">
    </asp:GridView>
    <div class="btn-group">
        <asp:Button ID="btnCsv" runat="server" Text="Export to CSV" CssClass="btn btn-success" OnClick="btnCsv_Click" />
        <asp:Button ID="btnPdf" runat="server" Text="Export to PDF" CssClass="btn btn-danger" OnClick="btnPdf_Click" />
    </div>
</asp:Content>
