using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Strover.Models;
using Strover.Application.Interfaces;

namespace Strover.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly Strover.Application.Interfaces.IOrderRepository _orders;

        public DetailsModel(IOrderRepository orders)
        {
            _orders = orders;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _orders.GetAsync(id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
