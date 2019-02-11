using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Strover.Models;

namespace Strover.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly Strover.Models.DataStoreContext _context;
        private readonly Strover.Application.Interfaces.IOrderService _service;

        public CreateModel(Strover.Models.DataStoreContext context,
                            Strover.Application.Interfaces.IOrderService service)
        {
            _context = context;
            _service = service;
        }


        public IActionResult OnGet()
        {
            //View like in the standard ordering leaflets, i.e. lines for each product.
            //We therefore create dummy order lines for each product
            var products = _context.Product.OrderBy(product => product.SequenceNumber).ToArray();
            ProductIds = products.Select(product => product.ProductId).ToList();
            ProductNames = products.Select(product => product.Name).ToList();
            OrderedQuantities = Enumerable.Repeat(element: 0u, count: ProductIds.Count).ToList();
            WillBePickedUp = false;
            WillBeDelivered = !WillBePickedUp;

            return Page();
        }

        [Required(ErrorMessage = "Name Buyer Should Be Filled")]
        [Display(Name = "Name Buyer")]
        [BindProperty]
        public string BuyerName { get; set; }

        [Required(ErrorMessage = "Telephone Number Should Be Filled")]
        [Display(Name = "Telephone Number")]
        [BindProperty]
        public string BuyerPhone { get; set; }

        // Group into Model
        [Display(Name = "Will be picked up on March 23 between 9h and 12h at kleuterschool Antoontje")]
        [BindProperty]
        public bool WillBePickedUp { get; set; }

        [Display(Name = "Will be delivered at home on March 23 between 9h and 13h at the address given below (only Sint-Antonius, Zoersel, Halle, Schilde or Westmalle")]
        [BindProperty]
        public bool WillBeDelivered { get; set; }


        [Display(Name = "Street")]
        [BindProperty]
        public string DeliveryStreet { get; set; }

        [Display(Name = "Number")]
        [BindProperty]
        public string DeliveryHouseNumber { get; set; }

        [Display(Name = "City")]
        [BindProperty]
        public string DeliveryCity { get; set; }

        [Display(Name = "Comments For The Delivery")]
        [BindProperty]
        public String DeliveryComments { get; set; }

        // Group into model
        [BindProperty]
        public List<String> ProductIds { get; set; }

        [BindProperty]
        public List<String> ProductNames { get; set; }

        [BindProperty]
        public List<uint> OrderedQuantities { get; set; } //Each request starts with a new instance of the view -> add id as hidden field with the quantity

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!OrderedQuantities.Any(selectedQuantity => selectedQuantity > 0))
                return Page();

            _service.RegisterOrder(asOrderRequest()); //&address &delivery or register order, add delivery & accept order? add action dimension

            return RedirectToPage("./Index");
        }

        private OrderRequest asOrderRequest()
        {
            var request = new OrderRequest();
            request.Buyer = new Customer()
            {
                Name = BuyerName,
                TelephoneNumber = BuyerPhone
            };
            request.Seller = new SalesPersonWrapper() { ID = User.Identity.Name };
            request.DeliveryMethod = createDeliveryDetails();
            request.Items = createItemSelection();

            return request;
        }

        private DeliveryMethod createDeliveryDetails()
        {
            var delivery = new DeliveryMethod()
            {
                DeliveryType = WillBePickedUp ? DeliveryType.Pickup : DeliveryType.Delivery,
                DeliveryAddress = new Address()
                {
                    Street = DeliveryStreet,
                    Number = DeliveryHouseNumber,
                    City = DeliveryCity
                },
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
