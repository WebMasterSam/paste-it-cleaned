using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("v1/clean")]
    [EnableCors("Default")]
    public class PasteCleanController : PasteItCleaned.Common.Controllers.PasteCleanController
    { }
}
