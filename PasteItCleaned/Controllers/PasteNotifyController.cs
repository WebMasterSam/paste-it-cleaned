using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/notify")]
    [ApiController]
    public class PasteNotifyController : ControllerBase
    {
        // POST api/v1/notify
        [HttpPost()]
        public ActionResult<string> Post([FromBody] NotifyObject obj)
        {
            // read api key from headers
            // simply add a stat for text only paste

            return "";
        }
    }

    public class NotifyObject
    {
        public string value { get; set; }
    }
}
