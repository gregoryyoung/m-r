<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<SimpleCQRS.InventoryItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<h2>Details:</h2>
Id: <%:Model.Id%><br />
Name: <%:Model.Name%><br />
Count: <%: Model.CurrentCount %><br /><br />

<%: Html.ActionLink("Rename","ChangeName", new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Deactivate","Deactivate",new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Check in","CheckIn", new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Remove","Remove", new{Id=Model.Id,Version=Model.Version}) %>
</asp:Content>
