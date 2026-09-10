<%@ Page Title="Employee Details & Notes" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="EmployeeDetail.aspx.cs" Inherits="EmployeeManagementSystem.Pages.EmployeeDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h3>Employee File:
                <asp:Literal ID="litEmployeeName" runat="server" /></h3>
            <a href="EmployeeList.aspx" class="btn btn-outline-secondary btn-sm">&larr; Back to Directory</a>
        </div>

        <asp:Label ID="lblNoteFeedback" runat="server" CssClass="alert d-block mb-3" Visible="false" />

        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-dark text-white fw-bold">
                <i class="bi bi-journal-text"></i>Log New Interaction / Case Note
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="form-label fw-semibold">Note Category</label>
                        <asp:DropDownList ID="ddlNoteType" runat="server" CssClass="form-select">
                            <asp:ListItem Text="General Note" Value="General" />
                            <asp:ListItem Text="Performance Review" Value="Performance" />
                            <asp:ListItem Text="Compliance Warning" Value="Compliance" />
                            <asp:ListItem Text="Onboarding Check-in" Value="Onboarding" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-12">
                        <label class="form-label fw-semibold">Note Details</label>
                        <asp:TextBox ID="txtNoteContent" runat="server" TextMode="MultiLine" Rows="3"
                            CssClass="form-control" placeholder="Enter notes or discussion summary..." />
                    </div>
                    <div class="col-12 text-end">
                        <asp:HiddenField ID="hfNoteId" runat="server" />
                        <asp:Button ID="btnAddNote" runat="server" Text="Save Case Note"
                            CssClass="btn btn-primary px-4" OnClick="btnAddNote_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Chronological Note Timeline -->
        <h5 class="fw-bold text-secondary mb-3">Interaction History</h5>
        

       <%-- Editing notes--%>
        <asp:Repeater ID="rptNotesTimeline" runat="server" OnItemCommand="rptNotesTimeline_ItemCommand">
       <%-- End Of editing notes--%>


            <ItemTemplate>
                <div class="card mb-2 border-start border-3 border-primary shadow-sm">
                    <div class="card-body py-2">

                        <%-- Editing the notes--%>
                        <div class="d-flex justify-content-between align-items-center">

                            <span class="badge bg-secondary">
                                <%# Eval("NoteType") %>
                            </span>

                            <div>
                                <small class="text-muted me-2">
                                    <%# Eval("CreatedAt", "{0:MMM dd, yyyy hh:mm tt}") %>
                                </small>

                                <asp:LinkButton ID="btnEditNote"
                                    runat="server"
                                    Text="Edit"
                                    CssClass="btn btn-sm btn-outline-primary"
                                    CommandName="EditNote"
                                    CommandArgument='<%# Eval("NoteId") %>' />
                            </div>

                        </div>

                        <%--EndOf Editing the Notes--%>
   <%--                     <div class="d-flex justify-content-between text-muted small">
                            <span class="badge bg-secondary"><%# Eval("NoteType") %></span>
                            <span><%# Eval("CreatedAt", "{0:MMM dd, yyyy hh:mm tt}") %></span>
                        </div>--%>
                        <p class="card-text mt-2 mb-1 text-dark"><%# Eval("NoteContent") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoNotes" runat="server" Text="No logged notes found for this employee."
            Visible="false" CssClass="text-muted fst-italic" />
    </div>
</asp:Content>
