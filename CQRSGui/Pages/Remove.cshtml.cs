using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class RemoveModel : PageModel
{
    private readonly ReadModelFacade readModel;
    private readonly FakeBus bus;

    public RemoveModel(ReadModelFacade readModel, FakeBus bus)
    {
        this.readModel = readModel;
        this.bus = bus;
    }
    
    public InventoryItemDetailsDto InventoryItem { get; set; }

    public void OnGet(Guid id)
    {
        InventoryItem = readModel.GetInventoryItemDetails(id);
    }
    
    public IActionResult OnPost(Guid id, int number, int version)
    {
        bus.Send(new RemoveItemsFromInventory(id, number, version));

        return RedirectToPage("./Index");
    }
}