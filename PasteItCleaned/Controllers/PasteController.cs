using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/clean")]
    [ApiController]
    public class PasteController : ControllerBase
    {
        // POST api/v1/clean
        [HttpPost()]
        public ActionResult<string> Post([FromBody] CleanObject obj)
        {
            // read api key from headers

            return string.Format("This is an HTML value <b>{0}</b> got from backend !!!", obj.value);
        }
    }

    public class CleanObject
    {
        public string value { get; set; }
    }
}
