using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class RenameModel : PageModel
{
    private readonly ReadModelFacade readModel;
    private readonly FakeBus bus;

    public RenameModel(ReadModelFacade readModel, FakeBus bus)
    {
        this.readModel = readModel;
        this.bus = bus;
    }
    
    public InventoryItemDetailsDto InventoryItem { get; set; }

    public void OnGet(Guid id)
    {
        InventoryItem = readModel.GetInventoryItemDetails(id);
    }
    
    public IActionResult OnPost(Guid id, string name, int version)
    {
        bus.Send(new RenameInventoryItem(id, name, version));

        return RedirectToPage("./Index");
    }
}