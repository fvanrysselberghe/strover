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
        private const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IOrderRepository _orderStore;
        private readonly DataStoreContext _context;

        private readonly UserManager<SalesPerson> _users;

        public IndexModel(IOrderRepository orderStore, DataStoreContext productStore, UserManager<SalesPerson> users)
        {
            _orderStore = orderStore;
            _context = productStore;
            _users = users;
        }

        public IList<Order> Order { get; set; } /// Orders that are active

        public async Task OnGetAsync()
        {
            Order = await PopulateMyListOfOrders();
        }

        private async Task<IList<Order>> PopulateMyListOfOrders()
        {
            if (User.IsInRole(ApplicationRole.Administrator))
                return await _orderStore.AllAsync();
            else
                return await _orderStore.AllForSellerAsync(User.Identity.Name);
        }

        ///<summary>
        /// Exports the order data to an Excel file
        ///</summary>
        public async Task<IActionResult> OnGetExportAsync()
        {
            Order = await PopulateMyListOfOrders();

            var excelFileContent = AsExcel(Order, GetProducts(), GetSellers());
            return new FileContentResult(excelFileContent, ContentTypeExcel);
        }

        private byte[] AsExcel(ICollection<Order> orders, List<Product> products, List<SalesPerson> sellers)
        {
            var document = new XSSFWorkbook();
            var sheet = document.CreateSheet("Details");

            CreateHeader(sheet, products);

            Int32 rowNum = 1;
            foreach (var order in orders)
            {
                var row = sheet.CreateRow(rowNum);
                row.CreateCell(0).SetCellValue(order.BuyerId);

                UInt16 colNbr = 0;
                row.CreateCell(colNbr++).SetCellValue(order.Buyer.Name);
                row.CreateCell(colNbr++).SetCellValue("");

                if (order.Delivery.DeliveryType == DeliveryType.Delivery)
                {
                    row.CreateCell(colNbr++).SetCellValue(order.Delivery.DeliveryAddress.Street);
                    row.CreateCell(colNbr++).SetCellValue(order.Delivery.DeliveryAddress.Number);
                    row.CreateCell(colNbr++).SetCellValue("");
                    row.CreateCell(colNbr++).SetCellValue(order.Delivery.DeliveryAddress.City);
                }
                else
                {
                    colNbr += 4;
                }

                row.CreateCell(colNbr++).SetCellValue(order.Buyer.TelephoneNumber);

                foreach (var product in products)
                {
                    var productItem = order.OrderedItems.FirstOrDefault(orderedItem => orderedItem.ProductId == product.ProductId);

                    if (productItem == null)
                        row.CreateCell(colNbr++).SetCellValue(0);
                    else
                        row.CreateCell(colNbr++).SetCellValue(productItem.Quantity);
                }
                row.CreateCell(colNbr++).SetCellValue(order.OrderedQuantity);
                row.CreateCell(colNbr++).SetCellValue(order.Cost.ToString());
                row.CreateCell(colNbr++).SetCellValue(order.Delivery.DeliveryType == DeliveryType.Pickup);
                row.CreateCell(colNbr++).SetCellValue(order.Delivery.Comments);

                var completeSeller = sellers.FirstOrDefault(seller => seller.UserName == order.SellerId);
                if (completeSeller != null)
                {
                    row.CreateCell(colNbr++).SetCellValue(completeSeller.Name);
                    row.CreateCell(colNbr++).SetCellValue(completeSeller.Class);
                }
                else
                {
                    row.CreateCell(colNbr++).SetCellValue("");
                    row.CreateCell(colNbr++).SetCellValue("");
                }


                ++rowNum;
            }

            var stream = new MemoryStream();
            document.Write(stream);

            // #TODO replace by better construct, e.g. temporary file?
            return stream.ToArray();
        }

        List<Product> GetProducts()
        {
            return _context.Product.OrderBy(product => product.SequenceNumber).ToList();
        }

        List<SalesPerson> GetSellers()
        {
            return _users.Users.ToList();
        }
        private void CreateHeader(NPOI.SS.UserModel.ISheet sheet, List<Product> products)
        {
            var row = sheet.CreateRow(0);

            UInt16 colNbr = 0;
            row.CreateCell(colNbr++).SetCellValue("koper_naam");
            row.CreateCell(colNbr++).SetCellValue("koper_voornaam");
            row.CreateCell(colNbr++).SetCellValue("koper_straat");
            row.CreateCell(colNbr++).SetCellValue("koper_straat_nr");
            row.CreateCell(colNbr++).SetCellValue("koper_bus");
            row.CreateCell(colNbr++).SetCellValue("koper_gemeente");
            row.CreateCell(colNbr++).SetCellValue("koper_telefoon");

            //, <producten>,totaal [#producten], totaal_betaald,	
            foreach (var product in products)
            {
                row.CreateCell(colNbr++).SetCellValue(product.Name);
            }
            row.CreateCell(colNbr++).SetCellValue("totaal");
            row.CreateCell(colNbr++).SetCellValue("totaal_betaald");
            row.CreateCell(colNbr++).SetCellValue("haalt_zelf_af");
            row.CreateCell(colNbr++).SetCellValue("opmerkingen");
            row.CreateCell(colNbr++).SetCellValue("verkoper_naam");
            row.CreateCell(colNbr++).SetCellValue("klas");
        }
    }
}
