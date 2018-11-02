using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using vlaaienslag.Models;

namespace vlaaienslag.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly vlaaienslag.Models.DataStoreContext _context;

        public IndexModel(vlaaienslag.Models.DataStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } /// Orders that are active

        public async Task OnGetAsync()
        {
            Order = await _context.Order.ToListAsync();
        }

        ///<summary>
        /// Exports the order data to an Excel file
        ///</summary>
        public IActionResult OnGetExport()
        {
            var document = new XSSFWorkbook();
            var sheet = document.CreateSheet("Details");

            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("naam");
            row.CreateCell(1).SetCellValue("address");

            row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue("piet");
            row.CreateCell(1).SetCellValue("heinkade");

            var stream = new MemoryStream();
            document.Write(stream);

            // #TODO replace by better construct, e.g. temporary file?
            var content = stream.ToArray();
            return new FileContentResult(content, ContentTypeExcel);
        }
    }
}
