using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using Strover.Application;
using Strover.Application.Interfaces;
using Strover.Infrastructure.Data;
using Strover.Models;

namespace Strover.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderStore;
        private readonly DataStoreContext _context;

        private readonly IStructuredReferenceFactory _referenceFactory;

        public IndexModel(IOrderRepository orderStore,
            DataStoreContext productStore,
            IStructuredReferenceFactory referenceFactory)
        {
            _orderStore = orderStore;
            _context = productStore;
            _referenceFactory = referenceFactory;
        }

        public IList<Order> Order { get; set; } /// Orders that are active

        public async Task OnGetAsync()
        {
            Order = await PopulateMyListOfOrders();
        }

        private async Task<IList<Order>> PopulateMyListOfOrders()
        {
            return await _orderStore.AllForSellerAsync(User.Identity.Name);
        }

        /// <summary>
        /// Starts the creation of a payment for our list of open orders by creating a "Pending"-payment.
        /// Later pages may move this state to an end state (e.g. Paid, or Cancelled). 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetPay()
        {
            //get orders which aren't paid yet
            var myOrders = await PopulateMyListOfOrders();
            ICollection<Order> ordersWithoutPayment = GetOrdersWithoutPayment(myOrders);

            if (ordersWithoutPayment.Count == 0)
            {
                Order = await PopulateMyListOfOrders();
                return Page();
            }

            decimal amountToPay = ordersWithoutPayment.Sum(order => order.Cost);

            //create a payment and assign to them
            var newPayment = new Payment()
            {
                Amount = amountToPay,
                Method = PaymentMethod.WireTransfer,
                State = PaymentState.BeingProcessed, //Can be confirmed later
                Reference = _referenceFactory.Create().AsPrintReference()
            };
            _context.Payment.Add(newPayment);

            //store the orders with their payment;
            foreach (var order in ordersWithoutPayment)
            {
                var orderPaymentLink = new OrderPayments()
                {
                    OrderId = order.OrderId,
                    PaymentId = newPayment.ID
                };
                _context.OrderPayment.Add(orderPaymentLink);
            }

            _context.SaveChanges();

            //redirect to payment page
            return RedirectToPage("/Payments/Wiretransfer/Pay", new
            {
                paymentId = newPayment.ID
            });
        }

        private ICollection<Order> GetOrdersWithoutPayment(IList<Order> ordersToCheck)
        {
            List<Order> withoutPayment = new List<Order>();

            foreach (var order in ordersToCheck)
            {
                if (order.Payments == null
                || order.Payments.Count() == 0
                || order.Payments.All(orderPayment => orderPayment.Payment.State == PaymentState.Cancelled))
                    withoutPayment.Add(order);
            }

            return withoutPayment;
        }
    }

}
