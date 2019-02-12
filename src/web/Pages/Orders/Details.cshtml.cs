using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Strover.Models;
using Strover.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Strover.Infrastructure.Data;

namespace Strover.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly Strover.Application.Interfaces.IOrderRepository _orders;
        private readonly UserManager<SalesPerson> _users;

        public DetailsModel(IOrderRepository orders, UserManager<SalesPerson> users)
        {
            _orders = orders;
            _users = users;
        }

        public Order Order { get; set; }
        public SalesPerson Seller { get; set; }

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

            Seller = await _users.FindByNameAsync(Order.SellerId);
            return Page();
        }
    }
}
