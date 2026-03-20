<%@ Page Title="Manage Movies" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageMovies.aspx.cs" Inherits="MovieTicketSystem.ManageMovies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4 align-items-center">
        <div class="col-12 col-md-6">
            <h2 class="fw-bold mb-0">Movie Catalog</h2>
            <p class="text-secondary">Manage and organize your cinema's movie collection</p>
        </div>
    </div>
    
    <div class="gridview-container mb-4">
        <h5 class="fw-bold mb-4"><i class="fas fa-edit me-2 text-primary"></i>Add / Edit Movie</h5>
        <asp:HiddenField ID="hfMovieId" runat="server" />
        <div class="row g-3">
            <div class="col-md-3">
                <label class="form-label">Title</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Inception"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label">Genre</label>
                <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control" placeholder="Sci-Fi"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="form-label">Language</label>
                <asp:TextBox ID="txtLanguage" runat="server" CssClass="form-control" placeholder="English"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="form-label">Duration (min)</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Number" placeholder="148"></asp:TextBox>
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnSave" runat="server" Text="Save Movie" CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
            </div>
        </div>
        <div class="mt-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25"></asp:Label>
        </div>
    </div>

    <div class="gridview-container">
        <h5 class="fw-bold mb-4"><i class="fas fa-list me-2 text-primary"></i>All Movies</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvMovies" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-hover" GridLines="None"
                DataKeyNames="MOVIE_ID" OnRowCommand="gvMovies_RowCommand">
                <Columns>
                    <asp:BoundField DataField="MOVIE_ID" HeaderText="ID" />
                    <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Title" />
                    <asp:BoundField DataField="GENRE" HeaderText="Genre" />
                    <asp:BoundField DataField="LANGUAGE" HeaderText="Language" />
                    <asp:BoundField DataField="DURATION" HeaderText="Duration" ItemStyle-CssClass="text-secondary" />
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-end" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditMovie" CommandArgument='<%# Eval("MOVIE_ID") %>' CssClass="btn btn-sm btn-outline-info me-2"><i class="fas fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteMovie" CommandArgument='<%# Eval("MOVIE_ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this movie?');"><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
