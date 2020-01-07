using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Backend.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class HitConfiguration : IEntityTypeConfiguration<Hit>
    {
        public void Configure(EntityTypeBuilder<Hit> builder)
        {
            builder.HasKey(m => m.HitId);

            //builder.Property(m => m.ClientId);
            //builder.Property(m => m.Date);
            //builder.Property(m => m.Hash);
            //builder.Property(m => m.Ip);
            //builder.Property(m => m.Price);
            //builder.Property(m => m.Referer);
            //builder.Property(m => m.Type);

            builder.ToTable("hit");
        }
    }
}
