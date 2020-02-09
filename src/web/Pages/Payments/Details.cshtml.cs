using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Strover.Models;

namespace Strover.Pages
{
    public class DetailsModel : PageModel
    {
        private DataStoreContext _context;

        public DetailsModel(DataStoreContext context)
        {
            _context = context;
        }

        public Payment Payment { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null)
                return NotFound();

            Payment = _context.Payment.Find(id);
            if (Payment == null)
                return NotFound();

            return Page();
        }
    }
}
