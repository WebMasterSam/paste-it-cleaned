using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Common.Localization;
using PasteItCleaned.Core.Services;

namespace PasteItCleaned.Backend.Common.Controllers
{
    [Route("billing")]
    public class BillingController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentMethodService _paymentMethodService;

        public BillingController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IInvoiceService invoiceService, IPaymentService paymentService, IPaymentMethodService paymentMethodService, ILogger<BillingController> logger) : base(apiKeyService, clientService, userService, logger)
        {
            this._invoiceService = invoiceService;
            this._paymentService = paymentService;
            this._paymentMethodService = paymentMethodService;
        }

        // GET billing/invoices/{id}
        [HttpGet("invoices/{id}")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetInvoice([FromHeader]string authorization)
        {
            Console.WriteLine("BillingController::GetInvoice");

            return Ok(T.Get("App.Up"));
        }

        // GET billing/invoices
        [HttpGet("invoices")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetInvoices([FromHeader]string authorization)
        {
            Console.WriteLine("BillingController::GetInvoices");

            return Ok(T.Get("App.Up"));
        }

        // GET billing/payment-method
        [HttpGet("payment-method")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult GetPaymentMethod([FromHeader]string authorization)
        {
            Console.WriteLine("BillingController::GetPaymentMethod");

            return Ok(T.Get("App.Up"));
        }

        // POST billing/payment-method
        [HttpPost("payment-method")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult Post([FromHeader]string authorization, [FromBody] BillingRequest obj)
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
