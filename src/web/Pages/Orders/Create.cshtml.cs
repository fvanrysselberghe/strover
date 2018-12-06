using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using vlaaienslag.Models;

namespace vlaaienslag.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly vlaaienslag.Models.DataStoreContext _context;
        private readonly vlaaienslag.Application.Interfaces.IOrderService _service;

        public CreateModel(vlaaienslag.Models.DataStoreContext context, 
                            vlaaienslag.Application.Interfaces.IOrderService service)
        {
            _context = context;
            _service = service;
        }


        public IActionResult OnGet()
        {
            //View like in the standard ordering leaflets, i.e. lines for each product.
            //We therefore create dummy order lines for each product
            ProductIds = _context.Product.Select(product => product.Name).ToList();
            OrderedQuantities = Enumerable.Repeat(element: 0u, count: ProductIds.Count).ToList();

            return Page();
        }

        [BindProperty]
        public Customer Buyer { get; set; }

        // Group into Model
        [BindProperty]
        public bool Pickup { get; set; }

        [BindProperty]
        public Address DeliveryAddress { get; set; }

        [BindProperty]
        public String DeliveryComments { get; set; }

        // Group into model
        [BindProperty]
        public List<String> ProductIds { get; set; }

        [BindProperty]
        public List<uint> OrderedQuantities { get; set; } //Each request starts with a new instance of the view -> add id as hidden field with the quantity


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _service.RegisterOrder(asOrderRequest()); //&address &delivery or register order, add delivery & accept order? add action dimension

            return RedirectToPage("./Index");
        }

        private OrderRequest asOrderRequest()
        {
            var request = new OrderRequest();
            request.Buyer = this.Buyer;
            request.Seller = new SalesPerson(){ID = User.Identity.Name};
            request.DeliveryMethod = createDeliveryDetails();
            request.Items = createItemSelection();

            return request;
        }

        private DeliveryMethod createDeliveryDetails()
        {
            var delivery = new DeliveryMethod()
            {
                DeliveryType = Pickup ? DeliveryType.Pickup : DeliveryType.Delivery,
                DeliveryAddress = DeliveryAddress,
                Comments = DeliveryComments
            };

            return delivery;
        }

        private IDictionary<string, uint> createItemSelection()
        {
            var itemSelection = new Dictionary<string, uint>();
            var nbrProducts = ProductIds.Count;
            for (var index = 0; index < nbrProducts; ++index)
            {
                if (OrderedQuantities[index] > 0)
                    itemSelection.Add(ProductIds[index], OrderedQuantities[index]);
            }

            return itemSelection;
        }
    }
}
