using System;

using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Common.Localization;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("billing")]
    public class BillingController : ControllerBase
    {
        // GET billing/invoices/
        [HttpGet("invoices/{id}")]
        public ActionResult GetInvoice()
        {
            Console.WriteLine("BillingController::GetInvoice");

            return Ok(T.Get("App.Up"));
        }

        // GET billing/invoices
        [HttpGet("invoices")]
        public ActionResult GetInvoices()
        {
            Console.WriteLine("BillingController::GetInvoices");

            return Ok(T.Get("App.Up"));
        }

        // GET billing/payment-method
        [HttpGet("payment-method")]
        public ActionResult GetPaymentMethod()
        {
            Console.WriteLine("BillingController::GetPaymentMethod");

            return Ok(T.Get("App.Up"));
        }

        // POST billing/payment-method
        [HttpPost("payment-method")]
        public ActionResult Post([FromBody] BillingRequest obj)
        {
            Console.WriteLine("BillingController::Post");

            return Ok(T.Get("App.Up"));
        }
    }

    public class BillingRequest
    {
        public string any { get; set; }
    }
}
