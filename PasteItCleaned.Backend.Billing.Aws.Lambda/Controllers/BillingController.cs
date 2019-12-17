using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.Billing.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("billing")]
    [EnableCors("Default")]
    public class BillingController : PasteItCleaned.Backend.Common.Controllers.AccountController
    { }
}
