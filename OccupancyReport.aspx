<%@ Page Title="Occupancy Performance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OccupancyReport.aspx.cs" Inherits="MovieTicketSystem.OccupancyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style>
    .occ-progress-bar {
        border-radius: 5px;
        background: linear-gradient(90deg, #4f46e5, #7c3aed);
    }
</style>
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Performance KPIs</h2>
            <p class="text-secondary">Analyze theater occupancy and throughput</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <div class="row align-items-end g-3">
            <div class="col-md-8">
                <label class="form-label fw-bold border-start border-success border-4 ps-2 mb-2">Select Production</label>
                <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-select form-select-lg"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Button ID="btnAnalyze" runat="server" Text="Execute Analysis" CssClass="btn btn-primary btn-lg w-100" OnClick="btnAnalyze_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlReport" runat="server" Visible="false">
        <div class="gridview-container">
            <h6 class="text-primary fw-bold text-uppercase mb-4" style="letter-spacing: 1px;">Top Performing Venues: <asp:Literal ID="litMovieTitle" runat="server"></asp:Literal></h6>
            <div class="table-responsive">
                <asp:GridView ID="gvOccupancy" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Rank">
                            <ItemTemplate>
                                <span class="badge rounded-circle bg-primary bg-opacity-10 text-primary d-inline-flex align-items-center justify-content-center" style="width: 30px; height: 30px;">
                                    <%# Container.DataItemIndex + 1 %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="THEATER_NAME" HeaderText="Venue" ItemStyle-CssClass="fw-bold" />
                        <asp:BoundField DataField="CAPACITY" HeaderText="Total Leads" />
                        <asp:BoundField DataField="PAID_TICKETS" HeaderText="Conversions" ItemStyle-CssClass="text-success fw-medium" />
                        <asp:TemplateField HeaderText="Occupancy Efficiency" ItemStyle-Width="30%">
                            <ItemTemplate>
                                <div class="progress bg-light" style="height: 10px; border-radius: 5px;">
                                    <div class="progress-bar occ-progress-bar" role="progressbar" 
                                        style='<%# "width: " + Eval("OCCUPANCY_PERCENT") + "%;" %>' 
                                        aria-valuenow='<%# Eval("OCCUPANCY_PERCENT") %>' aria-valuemin="0" aria-valuemax="100">
                                    </div>
                                </div>
                                <div class="d-flex justify-content-between mt-2">
                                    <small class="text-secondary"><%# string.Format("{0:0.0}% Capacity", Eval("OCCUPANCY_PERCENT")) %></small>
                                    <small class="text-primary fw-bold"><i class="fas fa-caret-up me-1"></i>Peak</small>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-chart-area fa-3x text-secondary mb-3 opacity-25"></i>
                            <p class="text-secondary">Insufficient data to generate performance intelligence for this production.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
