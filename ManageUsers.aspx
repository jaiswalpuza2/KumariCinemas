<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="MovieTicketSystem.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">User Directory</h2>
            <p class="text-secondary">Manage customer accounts and access</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <h5 class="fw-bold mb-4"><i class="fas fa-user-edit me-2 text-primary"></i>Add / Edit User</h5>
        <asp:HiddenField ID="hfUserId" runat="server" />
        <div class="row g-3">
            <div class="col-md-3">
                <label class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="john_doe"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="john@example.com"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+977-XXXXXXXXXX"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <label class="form-label">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Kathmandu, Nepal"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Role</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSave" runat="server" Text="Save User" CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25"></asp:Label>
        </div>
    </div>

    <div class="gridview-container">
        <h5 class="fw-bold mb-4"><i class="fas fa-users me-2 text-primary"></i>Registered Users</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" GridLines="None"
                DataKeyNames="USER_ID" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="USER_ID" HeaderText="ID" />
                    <asp:BoundField DataField="USERNAME" HeaderText="Username" ItemStyle-CssClass="fw-bold" />
                    <asp:BoundField DataField="EMAIL" HeaderText="Email" />
                    <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Phone" ItemStyle-CssClass="text-secondary" />
                    <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                    <asp:TemplateField HeaderText="Role">
                        <ItemTemplate>
                            <span class='<%# Eval("USER_ROLE").ToString() == "Admin" ? "badge bg-primary" : "badge bg-secondary" %>'>
                                <%# Eval("USER_ROLE") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditUser" CommandArgument='<%# Eval("USER_ID") %>' CssClass="btn btn-sm btn-outline-info me-2"><i class="fas fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("USER_ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this user?');"><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
