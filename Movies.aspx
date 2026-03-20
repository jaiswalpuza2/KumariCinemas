<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Movie.aspx.cs" Inherits="Movie" %>

<!DOCTYPE html>
<html>
<head runat="server">
<title>Movie</title>
</head>
<body>
<form id="form1" runat="server">

Movie Title:
<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>

Duration:
<asp:TextBox ID="txtDuration" runat="server"></asp:TextBox>

Language:
<asp:TextBox ID="txtLanguage" runat="server"></asp:TextBox>

Genre:
<asp:TextBox ID="txtGenre" runat="server"></asp:TextBox>

<asp:Button ID="btnAdd" runat="server" Text="Add Movie" OnClick="btnAdd_Click"/>

<br/><br/>

<asp:GridView ID="GridView1" runat="server"></asp:GridView>

</form>
</body>
</html>
