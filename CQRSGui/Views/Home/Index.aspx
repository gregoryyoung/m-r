<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SimpleCQRS.InventoryItemListDto>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All items:</h2>
    <ul><% foreach (var inventoryItemListDto in Model)
        {%><li>
            <%: Html.ActionLink("Name: " + inventoryItemListDto.Name,"Details",new{Id=inventoryItemListDto.Id}) %>
        </li>
    <%} %></ul>
    <%: Html.ActionLink("Add","Add") %>
</asp:Content>
