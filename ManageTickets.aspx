<%@ Page Title="Manage Tickets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageTickets.aspx.cs" Inherits="MovieTicketSystem.ManageTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Ticket Desk</h2>
            <p class="text-secondary">Manage bookings and seat assignments</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <h5 class="fw-bold mb-4"><i class="fas fa-ticket-alt me-2 text-primary"></i>Add / Edit Ticket</h5>
        <asp:HiddenField ID="hfTicketId" runat="server" />
        <div class="row g-3">
            <div class="col-md-3">
                <label class="form-label">User</label>
                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <label class="form-label">Showtime</label>
                <asp:DropDownList ID="ddlShowtimes" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label class="form-label">Seat Number</label>
                <asp:TextBox ID="txtSeat" runat="server" CssClass="form-control" placeholder="e.g. A1"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">Payment Status</label>
                <div class="row g-2">
                    <div class="col-8">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
                            <asp:ListItem Text="Unpaid" Value="Unpaid"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-4">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25"></asp:Label>
        </div>
    </div>

    <div class="gridview-container">
        <h5 class="fw-bold mb-4"><i class="fas fa-list me-2 text-primary"></i>All Tickets</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" GridLines="None"
                DataKeyNames="TICKET_ID" OnRowCommand="gvTickets_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TICKET_ID" HeaderText="ID" />
                    <asp:BoundField DataField="USERNAME" HeaderText="User" ItemStyle-CssClass="fw-medium" />
                    <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Movie" />
                    <asp:BoundField DataField="SHOW_TIME" HeaderText="Show Time" />
                    <asp:BoundField DataField="SEAT_NUMBER" HeaderText="Seat" ItemStyle-CssClass="text-secondary" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge <%# Eval("STATUS").ToString() == "Paid" ? "bg-success bg-opacity-10 text-success border border-success border-opacity-25" : "bg-warning bg-opacity-10 text-warning border border-warning border-opacity-25" %>'>
                                <%# Eval("STATUS") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditTicket" CommandArgument='<%# Eval("TICKET_ID") %>' CssClass="btn btn-sm btn-outline-info me-2"><i class="fas fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteTicket" CommandArgument='<%# Eval("TICKET_ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this ticket?');"><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
