using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Plugin.Controllers.Entities;

namespace PasteItCleaned.Plugin.Controllers
{
    [ApiController]
    [Route("v1/notify")]
    [EnableCors("Default")]
    public class PasteNotifyController : BaseController
    {
        public PasteNotifyController() : base()
        {

        }

        // POST v1/notify
        [HttpPost()]
        public ActionResult Post([FromBody] NotifyObject obj)
        {
            return Ok(new PluginSuccess(""));
        }
    }

    public class NotifyObject
    {
        public int hash { get; set; }
        public string pasteType { get; set; }
    }
}
