using System;

using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        // GET account
        [HttpGet()]
        public ActionResult GetAccount([FromHeader]string authorization)
        {
            Console.WriteLine("AccountController::Get");

            //var client = SessionHelper.GetCurrentClient(authorization);

            return Ok(T.Get("App.Up"));
        }

        // GET account/user
        [HttpGet("user")]
        public ActionResult GetUser([FromHeader]string authorization)
        {
            Console.WriteLine("AccountController::GetUser");

            //var client = SessionHelper.GetCurrentClient(authorization);

            return Ok(T.Get("App.Up"));
        }

        // POST account
        [HttpPost()]
        public ActionResult PostAccount([FromHeader]string authorization, [FromBody] AccountRequest obj)
        {
            Console.WriteLine("AccountController::Post");

            //var client = SessionHelper.GetCurrentClient(authorization);

            return Ok(T.Get("App.Up"));
        }
    }

    public class AccountRequest
    {
        public string any { get; set; }
    }
}
