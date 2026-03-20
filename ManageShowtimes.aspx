<%@ Page Title="Manage Showtimes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageShowtimes.aspx.cs" Inherits="MovieTicketSystem.ManageShowtimes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Show Scheduling</h2>
            <p class="text-secondary">Manage movie screenings and pricing</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <h5 class="fw-bold mb-4"><i class="fas fa-clock me-2 text-primary"></i>Add / Edit Showtime</h5>
        <asp:HiddenField ID="hfShowId" runat="server" />
        <div class="row g-3">
            <div class="col-md-3">
                <label class="form-label">Movie</label>
                <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label class="form-label">Theater</label>
                <asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label class="form-label">Time</label>
                <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="form-label">Price</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="10.00"></asp:TextBox>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSave" runat="server" Text="Save Schedule" CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25"></asp:Label>
        </div>
    </div>

    <div class="gridview-container">
        <h5 class="fw-bold mb-4"><i class="fas fa-calendar-alt me-2 text-primary"></i>Current Schedule</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvShowtimes" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" GridLines="None"
                DataKeyNames="SHOW_ID" OnRowCommand="gvShowtimes_RowCommand">
                <Columns>
                    <asp:BoundField DataField="SHOW_ID" HeaderText="ID" />
                    <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Movie" />
                    <asp:BoundField DataField="THEATER_NAME" HeaderText="Theater" />
                    <asp:BoundField DataField="SHOW_TIME" HeaderText="Show Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-CssClass="fw-medium" />
                    <asp:BoundField DataField="PRICE" HeaderText="Price" DataFormatString="{0:C}" ItemStyle-CssClass="text-success fw-bold" />
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditShow" CommandArgument='<%# Eval("SHOW_ID") %>' CssClass="btn btn-sm btn-outline-info me-2"><i class="fas fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteShow" CommandArgument='<%# Eval("SHOW_ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this showtime?');"><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
