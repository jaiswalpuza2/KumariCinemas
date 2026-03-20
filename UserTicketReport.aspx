<%@ Page Title="User Ticket Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTicketReport.aspx.cs" Inherits="MovieTicketSystem.UserTicketReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Ticket History</h2>
            <p class="text-secondary">Analyze user bookings over the last 6 months</p>
        </div>
    </div>
    
    <asp:PlaceHolder ID="phUserSelection" runat="server">
        <div class="gridview-container mb-4">
            <div class="row align-items-end g-3">
                <div class="col-md-8">
                    <label class="form-label fw-bold">Select Customer</label>
                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select form-select-lg"></asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnViewReport" runat="server" Text="Generate Intelligence Report" CssClass="btn btn-primary btn-lg w-100" OnClick="btnViewReport_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

    <asp:Panel ID="pnlReport" runat="server" Visible="false">
        <div class="gridview-container mb-4 border-primary border-opacity-10">
            <h6 class="text-primary fw-bold text-uppercase mb-3" style="letter-spacing: 1px;">Customer Profile</h6>
            <div class="row g-4">
                <div class="col-md-4">
                    <div class="p-3 rounded-3 bg-light">
                        <small class="text-secondary d-block mb-1">Username</small>
                        <span class="fw-bold"><asp:Literal ID="litUsername" runat="server"></asp:Literal></span>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="p-3 rounded-3 bg-light">
                        <small class="text-secondary d-block mb-1">Email Connection</small>
                        <span class="fw-bold"><asp:Literal ID="litEmail" runat="server"></asp:Literal></span>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="p-3 rounded-3 bg-light">
                        <small class="text-secondary d-block mb-1">Contact Number</small>
                        <span class="fw-bold"><asp:Literal ID="litPhone" runat="server"></asp:Literal></span>
                    </div>
                </div>
            </div>
        </div>

        <div class="gridview-container">
            <h6 class="text-primary fw-bold text-uppercase mb-3" style="letter-spacing: 1px;">Booking Ledger</h6>
            <div class="table-responsive">
                <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="TICKET_ID" HeaderText="ID" />
                        <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Movie" ItemStyle-CssClass="fw-bold" />
                        <asp:BoundField DataField="THEATER_NAME" HeaderText="Theater" />
                        <asp:BoundField DataField="SHOW_TIME" HeaderText="Screening Time" />
                        <asp:BoundField DataField="SEAT_NUMBER" HeaderText="Seat" ItemStyle-CssClass="text-secondary" />
                        <asp:BoundField DataField="BOOKING_DATE" HeaderText="Processed On" DataFormatString="{0:MMM dd, yyyy}" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='badge <%# Eval("STATUS").ToString() == "Paid" ? "bg-success bg-opacity-10 text-success border border-success border-opacity-25" : "bg-warning bg-opacity-10 text-warning border border-warning border-opacity-25" %>'>
                                    <%# Eval("STATUS") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-search fa-3x text-secondary mb-3 opacity-25"></i>
                            <p class="text-secondary">No historical ticket data found for this user in our 6-month vault.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
