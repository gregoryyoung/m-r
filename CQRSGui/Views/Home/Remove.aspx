<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<SimpleCQRS.InventoryItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
        <% using (Html.BeginForm())
       {%>
        <%: Html.Hidden("Id",Model.Id) %>
        <%: Html.Hidden("Version",Model.Version) %>
        Number:<%: Html.TextBox("Number") %><br />
        <button name="submit">Submit</button>

    <%
       }%>

</asp:Content>
