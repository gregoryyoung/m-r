<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<SimpleCQRS.InventoryItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
        <% using (Html.BeginForm())
       {%>
        <%: Html.Hidden("Id",Model.Id) %>
        <%: Html.Hidden("Version",Model.Version) %>
    Deactivate &quot;<%: Model.Name %>&quot; (Version: <%: Model.Version %>)?
        <button name="submit">Submit</button>

    <%
       }%>

</asp:Content>
