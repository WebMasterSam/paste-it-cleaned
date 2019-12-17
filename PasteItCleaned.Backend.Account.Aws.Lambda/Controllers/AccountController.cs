using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.Account.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("account")]
    [EnableCors("Default")]
    public class AccountController : PasteItCleaned.Backend.Common.Controllers.AccountController
    { }
}
