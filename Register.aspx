<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MovieTicketSystem.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="row justify-content-center align-items-center" style="min-height: 80vh;">
    <div class="col-md-6">
        <div class="gridview-container">
            <div class="text-center mb-4">
                <i class="fas fa-user-plus fa-3x text-primary mb-3"></i>
                <h2 class="fw-bold">Create Account</h2>
                <p class="text-secondary">Join the Kumari Cinema community</p>
            </div>

            <div class="row g-3">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="johndoe"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="john@example.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" CssClass="text-danger small"></asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>

            <div class="mb-3">
                <label class="form-label">Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+977-XXXXXXXXXX"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone number is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>

            <div class="mb-4">
                <label class="form-label">Full Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="City, Street, House No."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>

            <div class="d-grid gap-2">
                <asp:ValidationSummary ID="vsRegister" runat="server" CssClass="alert alert-danger" DisplayMode="BulletList" HeaderText="Please correct the following errors:" ShowMessageBox="false" ShowSummary="true" />
                <asp:Button ID="btnRegister" runat="server" Text="Register Now" OnClick="btnRegister_Click" CssClass="btn btn-primary btn-lg" />
            </div>

            <div class="text-center mt-4">
                <p class="text-secondary mb-0">Already have an account? <a href="Login" class="text-primary text-decoration-none fw-bold">Sign In</a></p>
            </div>

            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-danger bg-opacity-10 text-danger p-2 border border-danger border-opacity-25 w-100" style="display: none;"></asp:Label>
            </div>
        </div>
    </div>
</div>
<script>
    window.onload = function() {
        var lbl = document.getElementById('<%= lblMessage.ClientID %>');
        if (lbl && lbl.innerText.trim() !== "") {
            lbl.style.display = "block";
        }
    };
</script>
</asp:Content>
