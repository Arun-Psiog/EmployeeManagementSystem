<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-danger p-4 text-center my-5 shadow-sm">
        <h3 class="fw-bold">403 - Access Denied</h3>
        <p>Your current role does not have authorization to view this resource.</p>
        <a href="EmployeeList.aspx" class="btn btn-outline-danger">Back to Employee Directory</a>
    </div>
</asp:Content>
