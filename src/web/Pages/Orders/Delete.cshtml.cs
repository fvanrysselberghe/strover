using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Strover.Models;
using Strover.Application.Interfaces;

namespace Strover.Web.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository _context;

        public DeleteModel(IOrderRepository context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.GetAsync(id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _context.Remove(id);

            return RedirectToPage("./Index");
        }
    }
}