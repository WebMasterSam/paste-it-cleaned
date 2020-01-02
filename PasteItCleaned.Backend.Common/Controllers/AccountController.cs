using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    public class AccountController : ControllerBase
    {
        // GET account
        [HttpGet()]
        public ActionResult Get()
        {
            Console.WriteLine("AccountController::Get");

            return Ok(T.Get("App.Up"));
        }

        // GET account/user
        [HttpGet("user")]
        public ActionResult GetUser()
        {
            Console.WriteLine("AccountController::GetUser");

            return Ok(T.Get("App.Up"));
        }

        // POST account
        [HttpPost()]
        public ActionResult Post([FromBody] AccountRequest obj)
        {
            Console.WriteLine("AccountController::Post");

            return Ok(T.Get("App.Up"));
        }
    }

    public class AccountRequest
    {
        public string any { get; set; }
    }
}
