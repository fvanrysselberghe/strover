using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Strover.Pages.Orders
{
    public class ClosedModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}