using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Strover.Models;

namespace Strover.Pages.Administration.Products
{
    public class CreateModel : PageModel
    {
        private readonly Strover.Models.DataStoreContext _context;

        public CreateModel(Strover.Models.DataStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //although unlikely it can collide in parallel sessions
            //We'll resolve it later, e.g. GUI
            Product.SequenceNumber = (_context.Product.Count()) + 1;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}