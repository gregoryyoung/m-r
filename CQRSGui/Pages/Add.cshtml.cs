using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleCQRS_2;

namespace CQRSGui.Pages;

public class AddModel : PageModel
{
    private FakeBus bus;
    
    public AddModel(FakeBus bus)
    {
        this.bus = bus;
    }
    
    [BindProperty]
    public string Name { get; set; }
    
    public IActionResult OnGet() => Page();
    
    public IActionResult OnPost()
    {
        bus.Send(new CreateInventoryItem(Guid.NewGuid(), Name));

        return RedirectToPage("./Index");
    }
}