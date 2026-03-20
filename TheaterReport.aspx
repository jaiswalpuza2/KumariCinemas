<%@ Page Title="Theater Movie Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterReport.aspx.cs" Inherits="MovieTicketSystem.TheaterReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Venue Logistics</h2>
            <p class="text-secondary">Explore movie schedules for specific theater halls</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <div class="row align-items-end g-3">
            <div class="col-md-8">
                <label class="form-label fw-bold">Select Venue</label>
                <asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-select form-select-lg"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Button ID="btnViewReport" runat="server" Text="Request Schedule" CssClass="btn btn-primary btn-lg w-100" OnClick="btnViewReport_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlReport" runat="server" Visible="false">
        <div class="gridview-container mb-4 border-primary border-opacity-10">
            <h5 class="fw-bold mb-3"><i class="fas fa-info-circle me-2 text-primary"></i>Venue Insights: <asp:Literal ID="litTheaterName" runat="server"></asp:Literal></h5>
            <div class="d-flex gap-4">
                <div class="text-secondary"><i class="fas fa-map-marker-alt me-2"></i>Location: <span class="fw-medium text-primary"><asp:Literal ID="litLocation" runat="server"></asp:Literal></span></div>
                <div class="text-secondary"><i class="fas fa-users me-2"></i>Capacity: <span class="fw-medium text-primary"><asp:Literal ID="litCapacity" runat="server"></asp:Literal> seats</span></div>
            </div>
        </div>

        <div class="gridview-container">
            <h6 class="text-primary fw-bold text-uppercase mb-3" style="letter-spacing: 1px;">Scheduled Showtimes</h6>
            <div class="table-responsive">
                <asp:GridView ID="gvSchedule" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Movie Production" ItemStyle-CssClass="fw-bold" />
                        <asp:BoundField DataField="GENRE" HeaderText="Genre" />
                        <asp:BoundField DataField="LANGUAGE" HeaderText="Local" ItemStyle-CssClass="text-secondary small" />
                        <asp:BoundField DataField="SHOW_TIME" HeaderText="Showtime" ItemStyle-CssClass="fw-medium text-primary" />
                        <asp:BoundField DataField="PRICE" HeaderText="Entry Price" DataFormatString="{0:C}" ItemStyle-CssClass="text-success fw-bold" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="alert alert-warning border-0 bg-warning bg-opacity-10 text-warning text-center">
                            <i class="fas fa-exclamation-triangle me-2"></i>No screenings are currently scheduled for this venue.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
