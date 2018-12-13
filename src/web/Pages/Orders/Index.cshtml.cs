using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using vlaaienslag.Application;
using vlaaienslag.Application.Interfaces;
using vlaaienslag.Models;

namespace vlaaienslag.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IOrderRepository _orderStore;

        public IndexModel(IOrderRepository orderStore) => _orderStore = orderStore;

        public IList<Order> Order { get; set; } /// Orders that are active

        public async Task OnGetAsync()
        {
            if (User.IsInRole(ApplicationRole.Administrator))
                Order = await _orderStore.GetAsync();
            else
                Order = await _orderStore.GetAsync(User.Identity.Name);
        }

        ///<summary>
        /// Exports the order data to an Excel file
        ///</summary>
        public async Task<IActionResult> OnGetExportAsync()
        {
            ICollection<Order> orders;

            if (User.IsInRole(ApplicationRole.Administrator))
                orders = await _orderStore.GetAsync();
            else
                orders = await _orderStore.GetAsync(User.Identity.Name);

            var excelFileContent = AsExcel(orders);
            return new FileContentResult(excelFileContent, ContentTypeExcel);
        }

        private byte[] AsExcel(ICollection<Order> orders)
        {
            var document = new XSSFWorkbook();
            var sheet = document.CreateSheet("Details");

            CreateHeader(sheet);

            Int32 rowNum = 1;
            foreach (var order in orders)
            {
                var row = sheet.CreateRow(rowNum);
                row.CreateCell(0).SetCellValue(order.BuyerId);

                ++rowNum;
            }

            var stream = new MemoryStream();
            document.Write(stream);

            // #TODO replace by better construct, e.g. temporary file?
            return stream.ToArray();
        }
        private void CreateHeader(NPOI.SS.UserModel.ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("koper");
        }

    }
}
