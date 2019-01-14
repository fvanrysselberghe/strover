using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using Strover.Application;
using Strover.Application.Interfaces;
using Strover.Models;

namespace Strover.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IOrderRepository _orderStore;

        public IndexModel(IOrderRepository orderStore) => _orderStore = orderStore;

        public IList<Order> Order { get; set; } /// Orders that are active

        public async Task OnGetAsync()
        {
            Order = await PopulateMyListOfOrders();
        }

        private async Task<IList<Order>> PopulateMyListOfOrders()
        {
            if (User.IsInRole(ApplicationRole.Administrator))
                return await _orderStore.GetAsync();
            else
                return await _orderStore.GetAsync(User.Identity.Name);
        }

        ///<summary>
        /// Exports the order data to an Excel file
        ///</summary>
        public async Task<IActionResult> OnGetExportAsync()
        {
            Order = await PopulateMyListOfOrders();

            var excelFileContent = AsExcel(Order);
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

            //koper_naam, koper_voornaam, koper_straat, koper_straat_nr, koper_bus, koper_gemeente, koper_telefoon, <producten>,totaal [#producten], totaal_betaald,	haalt_zelf_af,	opmerkingen, verkoper_naam,	klas, verkoper, unified address ,geocode       }

        }
    }
}
