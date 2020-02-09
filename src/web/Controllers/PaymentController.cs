using Microsoft.AspNetCore.Mvc;
using Payconiq.Datamodel;

namespace Strover.Web.Api
{
    [Route("api/payconiq/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostPayment(CallbackRequest data)
        {
            return Ok();
        }

    }
}