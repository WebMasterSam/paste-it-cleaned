using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.IIS.Controllers
{
    [ApiController]
    [Route("analytics")]
    [EnableCors("Default")]
    [Authorize]
    public class AnalyticsController : PasteItCleaned.Backend.Common.Controllers.AnalyticsController
    { }
}
