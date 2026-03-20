<%@ Page Title="Add Movie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddMovie.aspx.cs" Inherits="MovieTicketSystem.AddMovie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Add Movie</h2>

Title:<br />
<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
<br /><br />

Duration:<br />
<asp:TextBox ID="txtDuration" runat="server"></asp:TextBox>
<br /><br />

Language:<br />
<asp:TextBox ID="txtLanguage" runat="server"></asp:TextBox>
<br /><br />

Genre:<br />
<asp:TextBox ID="txtGenre" runat="server"></asp:TextBox>
<br /><br />

<asp:Button ID="btnAdd" runat="server" Text="Add Movie" OnClick="btnAdd_Click" />

<br /><br />
<asp:Label ID="lblMessage" runat="server"></asp:Label>

</asp:Content>
