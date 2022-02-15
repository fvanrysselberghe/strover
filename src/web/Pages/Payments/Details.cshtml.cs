using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Strover.Models;

namespace Strover.Pages
{
    public class DetailsModel : PageModel
    {
        private DataStoreContext _context;
        private ShopOptions _config;

        public DetailsModel(DataStoreContext context, IOptions<ShopOptions> shopConfiguration)
        {
            _context = context;

            _config = shopConfiguration.Value;
        }

        public Payment Payment { get; set; }

        public String Beneficiary => _config.AccountNumber;

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
