using PasteItCleaned.Core.Entities;
using System;

namespace PasteItCleaned.Core.Helpers
{
    public static class PricingHelper
    {
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
