using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.IIS.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    [Authorize]
    public class PluginController : PasteItCleaned.Backend.Common.Controllers.PluginController
    { }
}
