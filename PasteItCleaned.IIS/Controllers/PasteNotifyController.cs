using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("v1/notify")]
    [EnableCors("Default")]
    public class PasteNotifyController : PasteItCleaned.Common.Controllers.PasteNotifyController
    { }
}
