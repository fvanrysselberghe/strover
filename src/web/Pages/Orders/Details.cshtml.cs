using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Strover.Models;
using Strover.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Strover.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace Strover.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly Strover.Application.Interfaces.IOrderRepository _orders;
        private readonly UserManager<SalesPerson> _users;
        private readonly ShopOptions _configuration;
        public DetailsModel(IOrderRepository orders, UserManager<SalesPerson> users, IOptions<ShopOptions> shopConfig)
        {
            _orders = orders;
            _users = users;
            _configuration = shopConfig.Value;
        }

        public Order Order { get; set; }
        public SalesPerson Seller { get; set; }

        public String DeliveryPeriod => _configuration.DeliveryPeriod.ToString();
        public String PickupLocations => _configuration.PickupLocations;
        public String PickupPeriod => _configuration.PickupPeriod.ToString();

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
