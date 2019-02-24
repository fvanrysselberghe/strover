using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Strover.Models;
using Strover.Application.Interfaces;

namespace Strover.Pages.Payments
{
    public class PaidModel : PageModel
    {
        private readonly DataStoreContext _context;

        public PaidModel(DataStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Payment Payment { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Payment = await _context.Payment.FindAsync(id);

            if (Payment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
                return NotFound();

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
                return NotFound();

            payment.State = PaymentState.Paid;
            _context.Payment.Update(payment);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}