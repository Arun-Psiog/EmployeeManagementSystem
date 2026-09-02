<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePassword.aspx.cs" Inherits="EmployeeManagementSystem.Pages.CreatePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Password - EMS</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light d-flex align-items-center justify-content-center" style="min-height:100vh;">

    <form id="form1" runat="server" style="width:380px;">

        <div class="card shadow-lg border-0">
            <div class="card-body p-4">

                <h3 class="text-center mb-4 fw-bold text-primary">
                    Create Password
                </h3>

                <div class="mb-3">
                    <label class="form-label fw-semibold">
                        Username
                    </label>

                    <asp:TextBox ID="TextBox2"
                        runat="server"
                        TextMode="SingleLine"
                        CssClass="form-control"
                        placeholder="Enter Username">
                    </asp:TextBox>
                </div>

                <div class="mb-4">
                    <label class="form-label fw-semibold">
                        New Password
                    </label>

                    <asp:TextBox ID="txtnewPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control"
                        placeholder="Enter New Password">
                    </asp:TextBox>
                </div>

                <asp:Button ID="btnCreate"
                    runat="server"
                    Text="Create Password"
                    CssClass="btn btn-primary w-100"
                    OnClick="btnCreate_Click" />

                <a class="nav-link" href="/Pages/Login.aspx">Back To Login</a>

            </div>
        </div>

    </form>

</body>
</html>