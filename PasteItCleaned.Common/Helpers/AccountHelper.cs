using System;
using PasteItCleaned.Common.Cleaners;

namespace PasteItCleaned.Common.Helpers
{
    public static class AccountHelper
    {
        public static bool BalanceIsSufficient(Guid clientId)
        {
            if (!ConfigHelper.GetAppSetting<bool>("Features.AccountValidation"))
                return true;

            var client = DbHelper.SelectClient(clientId);

            return client.Billing.Balance > 0;
        }

        public static void DecreaseBalance(Guid clientId, SourceType type, decimal decreaseBy)
        {
            DbHelper.DecreaseClientBalance(clientId, decreaseBy);
        }

        public static decimal GetHitPrice(Guid clientId, SourceType type)
        {
            var priceText = ConfigHelper.GetAppSettingDecimal("Pricing.Text");
            var priceImage = ConfigHelper.GetAppSettingDecimal("Pricing.Image");
            var priceOffice = ConfigHelper.GetAppSettingDecimal("Pricing.Office");
            var priceWeb = ConfigHelper.GetAppSettingDecimal("Pricing.Web");
            var decreaseBy = 0.0M;

            switch (type)
            {
                case SourceType.Excel: decreaseBy = priceOffice; break;
                case SourceType.PowerPoint: decreaseBy = priceOffice; break;
                case SourceType.Word: decreaseBy = priceOffice; break;
                case SourceType.Image: decreaseBy = priceImage; break;
                case SourceType.Text: decreaseBy = priceText; break;
                case SourceType.Web: decreaseBy = priceWeb; break;
            }

            return decreaseBy;
        }
    }
}
