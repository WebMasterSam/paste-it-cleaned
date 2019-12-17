using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Backend.Plugin.Aws.Lambda.Controllers
{
    [ApiController]
    [Route("plugin")]
    [EnableCors("Default")]
    public class PluginController : PasteItCleaned.Backend.Common.Controllers.PluginController
    { }
}
