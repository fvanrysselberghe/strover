using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Strover.Infrastructure.Data;
using Strover.Models;

namespace Strover.Pages.Payments
{
    public class IndexModel : PageModel
    {
        private readonly DataStoreContext _store;

        public IndexModel(DataStoreContext paymentStore)
        {
            _store = paymentStore;
        }

        public IList<Payment> Payments { get; set; } /// Payments that are active

        public async Task OnGetAsync()
        {
            Payments = await GetPayments();
        }

        private async Task<IList<Payment>> GetPayments()
        {
            return await _store.Payment.ToListAsync();
        }



    }

}
