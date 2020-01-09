using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class HitDailyConfiguration : IEntityTypeConfiguration<HitDaily>
    {
        public void Configure(EntityTypeBuilder<HitDaily> builder)
        {
            builder.HasKey(m => m.HitDailyId);

            builder.ToTable("hit_daily");
        }
    }
}
