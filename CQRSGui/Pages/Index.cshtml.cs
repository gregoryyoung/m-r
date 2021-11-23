using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class IndexModel : PageModel
{
    private readonly ReadModelFacade readModel;

    public IndexModel(ReadModelFacade readModel)
    {
        this.readModel = readModel;
    }

    public IActionResult OnGet()
    {
        InventoryItems = readModel.GetInventoryItems();
        return Page();
    }

    public IEnumerable<InventoryItemListDto> InventoryItems { get; set; }
}