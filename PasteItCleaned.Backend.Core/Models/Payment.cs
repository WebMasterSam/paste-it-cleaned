﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Payment
    {
        [Column("payment_id")]
        public Guid PaymentId { get; set; }

        [Column("invoice_id")]
        public Guid InvoiceId { get; set; }

        [Column("payment_method_id")]
        public Guid PaymentMethodId { get; set; }

        [Column("trx_number")]
        public string TrxNumber { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
