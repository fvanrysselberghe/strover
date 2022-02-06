using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Strover.Models;

namespace Strover.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly Strover.Models.DataStoreContext _context;
        private readonly Strover.Application.Interfaces.IOrderService _service;

        private readonly ShopOptions _configuration;

        public CreateModel(Strover.Models.DataStoreContext context,
                            Strover.Application.Interfaces.IOrderService service,
                            IOptions<ShopOptions> config)
        {
            _context = context;
            _service = service;
            _configuration = config.Value;
        }


        public IActionResult OnGet()
        {
            if (_configuration.Closed)
                return RedirectToPage("Closed");

            //View like in the standard ordering leaflets, i.e. lines for each product.
            //We therefore create dummy order lines for each product
            var products = _context.Product.OrderBy(product => product.SequenceNumber).ToArray();

            ItemView = products.Select(product => new ItemModel()
            {
                ProductName = product.Name,
                ProductId = product.ProductId,
                Quantity = 0,
                Price = product.Price,
                ImageLocation = product.ImageLocation
            }).ToList();

            WillBePickedUp = false;
            WillBeDelivered = !WillBePickedUp;

            return Page();
        }

        public class ItemModel
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }

            [Range(0, int.MaxValue)]
            public uint Quantity { get; set; }

            public decimal Price { get; set; }

            public string ImageLocation { get; set; }
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
        [BindProperty]
        public bool WillBePickedUp { get; set; }

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

        public String DeliveryPeriod
        {
            get
            {
                return _configuration.DeliveryPeriod.ToString();
            }
        }

        public String DeliveryLocations => _configuration.DeliveryLocations;

        public String PickupPeriod
        {
            get
            {
                return _configuration.PickupPeriod.ToString();
            }
        }

        public String PickupLocations => _configuration.PickupLocations;


        [BindProperty]
        [AtLeastOneItem(ErrorMessage = "At leat one item should be selected")]
        public List<ItemModel> ItemView { get; set; }

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

            foreach (var item in ItemView)
            {
                if (item.Quantity > 0)
                    itemSelection.Add(item.ProductId, item.Quantity);
            }

            return itemSelection;
        }
    }
}
