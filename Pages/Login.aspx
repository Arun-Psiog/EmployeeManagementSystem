<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmployeeManagementSystem.Pages.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login - EMS</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light d-flex align-items-center justify-content-center" style="min-height: 100vh;">
    <form id="form1" runat="server" style="width: 360px;">
        <div class="card p-4 shadow-sm">
            <h4 class="text-center mb-3 fw-bold">Sign In</h4>
            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
            <div class="mb-3">
                <label class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
            <a class="nav-link" href="/Pages/CreatePassword.aspx">Create Password</a>
        </div>
    </form>
</body>
</html>
