using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Strover.Models;

namespace Strover.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Strover.Models.DataStoreContext _context;

        public IndexModel(Strover.Models.DataStoreContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }
    }
}
