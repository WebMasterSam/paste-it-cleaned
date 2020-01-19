using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasteItCleaned.Backend.Entities;
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

        public BillingController(IApiKeyService apiKeyService, IClientService clientService, IUserService userService, IInvoiceService invoiceService, IPaymentService paymentService, IPaymentMethodService paymentMethodService, IConfigService configService, ILogger<BillingController> logger) : base(apiKeyService, clientService, userService, configService, logger)
        {
            this._invoiceService = invoiceService;
            this._paymentService = paymentService;
            this._paymentMethodService = paymentMethodService;
        }

        // GET billing/invoices/{id}
        [HttpGet("invoices/{id}")]
        [ProducesResponseType(typeof(InvoiceEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<InvoiceEntity> GetInvoice([FromHeader]string authorization)
        {
            Console.WriteLine("BillingController::GetInvoice");

            return Ok(T.Get("App.Up"));
        }

        // GET billing/invoices
        [HttpGet("invoices")]
        [ProducesResponseType(typeof(ListInvoiceEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<ListInvoiceEntity> GetInvoices([FromHeader]string authorization)
        {
            var client = this.GetOrCreateClient(authorization);

            if (client == null)
                return StatusCode(401);

            try
            {
                var invoices = _invoiceService.List(client.ClientId, 1, 200);

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return this.LogAndReturn500(ex);
            }
        }

        // GET billing/payment-method
        [HttpGet("payment-method")]
        [ProducesResponseType(typeof(PaymentMethodEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<PaymentMethodEntity> GetPaymentMethod([FromHeader]string authorization)
        {
            Console.WriteLine("BillingController::GetPaymentMethod");

            return Ok(T.Get("App.Up"));
        }

        // POST billing/payment-method
        [HttpPost("payment-method")]
        [ProducesResponseType(typeof(PaymentMethodEntity), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<PaymentMethodEntity> PostPaymentMethod([FromHeader]string authorization, [FromBody] BillingRequest obj)
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
