<%@ Page Title="Manage Theaters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageTheaters.aspx.cs" Inherits="MovieTicketSystem.ManageTheaters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Theater Venues</h2>
            <p class="text-secondary">Configure hall capacities and locations</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <h5 class="fw-bold mb-4"><i class="fas fa-building me-2 text-primary"></i>Add / Edit Theater</h5>
        <asp:HiddenField ID="hfTheaterId" runat="server" />
        <div class="row g-3">
            <div class="col-md-4">
                <label class="form-label">Theater Name</label>
                <asp:TextBox ID="txtTheaterName" runat="server" CssClass="form-control" placeholder="Hall 01"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label">Location</label>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="Floor 2, South Wing"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="form-label">Capacity</label>
                <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control" TextMode="Number" placeholder="200"></asp:TextBox>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSave" runat="server" Text="Save Theater" CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25"></asp:Label>
        </div>
    </div>

    <div class="gridview-container">
        <h5 class="fw-bold mb-4"><i class="fas fa-list me-2 text-primary"></i>All Theaters</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvTheaters" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" GridLines="None"
                DataKeyNames="THEATER_ID" OnRowCommand="gvTheaters_RowCommand">
                <Columns>
                    <asp:BoundField DataField="THEATER_ID" HeaderText="ID" />
                    <asp:BoundField DataField="THEATER_NAME" HeaderText="Theater Name" />
                    <asp:BoundField DataField="LOCATION" HeaderText="Location" />
                    <asp:BoundField DataField="CAPACITY" HeaderText="Capacity" ItemStyle-CssClass="text-secondary" />
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditTheater" CommandArgument='<%# Eval("THEATER_ID") %>' CssClass="btn btn-sm btn-outline-info me-2"><i class="fas fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteTheater" CommandArgument='<%# Eval("THEATER_ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this theater?');"><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
