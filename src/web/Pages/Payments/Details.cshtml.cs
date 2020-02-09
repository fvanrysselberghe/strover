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

        DetailsModel(DataStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string paymentId)
        {
            var payment = _context.Payment.Find(paymentId);
            if (payment == null)
                return NotFound();
            return Page();
        }
    }
}
