<%@ Page Title="Bulk Upload" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="BulkEmployeeUpload.aspx.cs" Inherits="EmployeeManagementSystem.Pages.BulkEmployeeUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Bulk Employee Upload</h3>
    <asp:Label ID="lblStatus" runat="server" CssClass="alert alert-info d-block" Visible="false"></asp:Label>
    <div class="card p-4 mb-4 shadow-sm">
        <label class="form-label fw-bold">Select CSV File (Format: EmployeeCode, FirstName, LastName, Email, Phone, DepartmentId)</label>
        <div class="input-group">
            <asp:FileUpload ID="fileUploadCsv" runat="server" CssClass="form-control" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload & Enqueue" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
        </div>
    </div>
    <h4>Uploaded Batch Jobs</h4>
    <asp:GridView ID="gvJobs" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" OnRowCommand="gvJobs_RowCommand">
        <Columns>
            <asp:BoundField DataField="JobId" HeaderText="Job ID" />
            <asp:BoundField DataField="FileName" HeaderText="File Name" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="ProcessedRows" HeaderText="Processed Rows" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Date Queued" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:TemplateField HeaderText="Rollback">
    <ItemTemplate>
        <asp:Button
            ID="btnRollback"
            runat="server"
            Text="Rollback"
            CssClass="btn btn-sm btn-warning" />
    </ItemTemplate>
</asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
