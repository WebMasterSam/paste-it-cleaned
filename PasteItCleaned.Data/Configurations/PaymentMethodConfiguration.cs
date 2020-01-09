using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(m => m.PaymentMethodId);

            builder.ToTable("payment_method");
        }
    }
}
