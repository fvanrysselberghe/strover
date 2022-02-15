using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Strover.Models;

namespace Strover.Pages.Payments
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

        private Payment Payment { get; set; }

        [Display(Name = "Beneficiary")]
        public String Beneficiary => _config.AccountNumber;

        [Display(Name = "Amount")]
        public decimal Amount => Payment.Amount;

        [Display(Name = "Reference")]
        public String Reference => Payment.Reference;

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
