<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MovieTicketSystem.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% if (Session["UserRole"] != null) { Response.Redirect("Default.aspx"); } %>
<div class="row justify-content-center align-items-center" style="min-height: 70vh;">
    <div class="col-md-5">
        <div class="gridview-container">
            <div class="text-center mb-4">
                <i class="fas fa-lock-open fa-3x text-primary mb-3"></i>
                <h2 class="fw-bold">Welcome Back</h2>
                <p class="text-secondary">Please enter your credentials to continue</p>
            </div>

            <div class="mb-3">
                <label class="form-label">Email Address</label>
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0 border-opacity-10 text-secondary"><i class="fas fa-envelope"></i></span>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control border-start-0" placeholder="name@example.com"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>

            <div class="mb-4">
                <label class="form-label">Password</label>
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0 border-opacity-10 text-secondary"><i class="fas fa-key"></i></span>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control border-start-0" placeholder="••••••••"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>

            <div class="d-grid gap-2">
                <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" CssClass="btn btn-primary btn-lg" />
            </div>

            <div class="text-center mt-4">
                <p class="text-secondary mb-0">Don't have an account? <a href="Register" class="text-primary text-decoration-none fw-bold">Sign Up</a></p>
            </div>

            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-danger bg-opacity-10 text-danger p-2 border border-danger border-opacity-25 w-100" style="display: none;"></asp:Label>
            </div>
        </div>
    </div>
</div>
<script>
    // Show/hide message label based on text
    window.onload = function() {
        var lbl = document.getElementById('<%= lblMessage.ClientID %>');
        if (lbl && lbl.innerText.trim() !== "") {
            lbl.style.display = "block";
        }
    };
</script>
</asp:Content>
