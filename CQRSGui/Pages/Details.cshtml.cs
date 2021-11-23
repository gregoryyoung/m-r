using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class DetailsModel : PageModel
{
    private readonly ReadModelFacade readModel;

    public DetailsModel(ReadModelFacade readModel)
    {
        this.readModel = readModel;
    }
    
    public InventoryItemDetailsDto InventoryItem { get; set; }

    public void OnGet(Guid id)
    {
        InventoryItem = readModel.GetInventoryItemDetails(id);
    }
}