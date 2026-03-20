<%@ Page Title="Book Ticket" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookTicket.aspx.cs" Inherits="MovieTicketSystem.BookTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center py-5">
        <div class="col-12 col-md-6 col-lg-5">
            <div class="gridview-container shadow-lg border-primary border-opacity-10">
                <div class="text-center mb-4">
                    <div class="display-6 fw-bold mb-2">Book Your Seat</div>
                    <p class="text-secondary">Secure your spot for the next big screening</p>
                </div>

                <div class="mb-4">
                    <label class="form-label d-flex align-items-center"><i class="fas fa-video me-2 text-primary"></i>Select Showtime</label>
                    <asp:DropDownList ID="ddlShowtime" runat="server" CssClass="form-select form-select-lg" AutoPostBack="false"></asp:DropDownList>
                </div>

                <div class="mb-4">
                    <label class="form-label d-flex align-items-center"><i class="fas fa-couch me-2 text-primary"></i>Seat Number</label>
                    <asp:TextBox ID="txtSeat" runat="server" CssClass="form-control form-control-lg" placeholder="e.g., A-12"></asp:TextBox>
                    <small class="text-secondary mt-1 d-block italic">Choose your preferred seat from the hall layout.</small>
                </div>

                <div class="mb-4">
                    <label class="form-label d-flex align-items-center"><i class="fas fa-tag me-2 text-primary"></i>Price Estimate</label>
                    <div class="input-group">
                        <span class="input-group-text bg-transparent border-end-0 text-secondary">$</span>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control form-control-lg border-start-0" placeholder="0.00"></asp:TextBox>
                    </div>
                </div>

                <div class="d-grid gap-2">
                    <asp:Button ID="btnBook" runat="server" Text="Confirm Booking" CssClass="btn btn-primary btn-lg py-3 fw-bold" OnClick="btnBook_Click" />
                </div>

                <div class="mt-4 text-center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 w-100 border border-success border-opacity-20"></asp:Label>
                </div>
            </div>
            
            <div class="mt-4 text-center">
                <a href="~/" runat="server" class="text-secondary text-decoration-none small hover-white">
                    <i class="fas fa-arrow-left me-1"></i>Back to Home
                </a>
            </div>
        </div>
    </div>
</asp:Content>
