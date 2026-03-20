<%@ Page Title="Database Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="MovieTicketSystem.Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center py-5">
        <div class="col-12 col-md-8 col-lg-6">
            <div class="gridview-container shadow-lg text-center p-5 border-primary border-opacity-10">
                <div class="mb-4">
                    <div class="display-5 text-primary mb-3"><i class="fas fa-database"></i></div>
                    <h2 class="fw-bold mb-2">System Initialization</h2>
                    <p class="text-secondary mb-4">Prepare your Oracle environment by generating core schema architecture and baseline data sets.</p>
                </div>

                <div class="p-4 rounded-4 bg-light mb-4 border border-dark border-opacity-5">
                    <div class="d-flex align-items-center mb-3">
                        <div class="badge bg-primary bg-opacity-10 text-primary me-3 p-2"><i class="fas fa-microchip"></i></div>
                        <div class="text-start">
                            <div class="fw-bold">Automated Schema Deployment</div>
                            <small class="text-secondary text-wrap text-start">Tables, sequences, and relationships will be verified and created.</small>
                        </div>
                    </div>
                    <div class="d-flex align-items-center">
                        <div class="badge bg-primary bg-opacity-10 text-primary me-3 p-2"><i class="fas fa-vial"></i></div>
                        <div class="text-start">
                            <div class="fw-bold">Baseline Data Injection</div>
                            <small class="text-secondary">Sample movies and theaters will be populated automatically.</small>
                        </div>
                    </div>
                </div>

                <div class="d-grid gap-3">
                    <asp:Button ID="btnSetup" runat="server" Text="Execute Full System Setup" CssClass="btn btn-primary btn-lg py-3 fw-bold shadow-lg" OnClick="btnSetup_Click" />
                </div>
                
                <div class="mt-4 text-start">
                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                </div>

                <div class="mt-5 pt-3 border-top border-white border-opacity-5">
                    <p class="text-secondary small mb-0">Connected to Instance: <span class="fw-medium text-primary">xepdb1</span></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
