using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using vlaaienslag.Models;

namespace vlaaienslag.Pages.Orders
{
    public class IndexModel : PageModel
    {
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
    }
}
