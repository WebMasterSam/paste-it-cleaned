namespace PasteItCleaned.Common.Entities
{
    public class Billing
    {
        public decimal Balance { get; set; }
        public Contact Contact { get; set; }
        public BillingPayPal PayPal { get; set; }
        public BillingStripe Stripe { get; set; }
    }
}
