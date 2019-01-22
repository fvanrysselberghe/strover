using Microsoft.AspNetCore.Identity;

namespace Strover.Infrastructure.Data
{
    public class SalesPerson : IdentityUser
    {
        public SalesPerson(string userName) : base(userName) { }

        public SalesPerson() : base() { }

        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Class { get; set; }
    }
}