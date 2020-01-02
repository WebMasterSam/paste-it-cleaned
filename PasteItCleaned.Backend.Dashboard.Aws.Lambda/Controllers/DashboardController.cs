using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.Dashboard.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("dashboard")]
    [EnableCors("Default")]
    [Authorize]
    public class DashboardController : PasteItCleaned.Backend.Common.Controllers.DashboardController
    { }
}
