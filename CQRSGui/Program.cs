using CQRSGui;
using Microsoft.AspNetCore.Builder;
using SimpleCQRS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var bus = new FakeBus();

var storage = new EventStore(bus);
var rep = new Repository<InventoryItem>(storage);
var commands = new InventoryCommandHandlers(rep);
bus.RegisterHandler<CheckInItemsToInventory>(commands.Handle);
bus.RegisterHandler<CreateInventoryItem>(commands.Handle);
bus.RegisterHandler<DeactivateInventoryItem>(commands.Handle);
bus.RegisterHandler<RemoveItemsFromInventory>(commands.Handle);
bus.RegisterHandler<RenameInventoryItem>(commands.Handle);
bus.RegisterHandler<ChangeMaxQty>(commands.Handle);
var detail = new InventoryItemDetailView();
bus.RegisterHandler<InventoryItemCreated>(detail.Handle);
bus.RegisterHandler<InventoryItemDeactivated>(detail.Handle);
bus.RegisterHandler<InventoryItemRenamed>(detail.Handle);
bus.RegisterHandler<ItemsCheckedInToInventory>(detail.Handle);
bus.RegisterHandler<ItemsRemovedFromInventory>(detail.Handle);
bus.RegisterHandler<MaxQtyChanged>(detail.Handle);
var list = new InventoryListView();
bus.RegisterHandler<InventoryItemCreated>(list.Handle);
bus.RegisterHandler<InventoryItemRenamed>(list.Handle);
bus.RegisterHandler<InventoryItemDeactivated>(list.Handle);
ServiceLocator.Bus = bus;

app.Run();
