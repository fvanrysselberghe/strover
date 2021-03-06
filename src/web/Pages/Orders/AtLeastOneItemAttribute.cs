using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Strover.Pages.Orders
{
    public class AtLeastOneItemAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList<CreateModel.ItemModel>;
            if (list == null)
                return false;

            return list.Any(v => v.Quantity > 0);
        }
    }


}