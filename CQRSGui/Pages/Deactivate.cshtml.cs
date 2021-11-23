using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class DeactivateModel : PageModel
{
    private readonly ReadModelFacade readModel;
    private readonly FakeBus bus;

    public DeactivateModel(ReadModelFacade readModel, FakeBus bus)
    {
        this.readModel = readModel;
        this.bus = bus;
    }
    
    public InventoryItemDetailsDto InventoryItem { get; set; }

    public void OnGet(Guid id)
    {
        InventoryItem = readModel.GetInventoryItemDetails(id);
    }
    
    public IActionResult OnPost(Guid id, int version)
    {
        bus.Send(new DeactivateInventoryItem(id, version));

        return RedirectToPage("./Index");
    }
}