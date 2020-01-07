using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Backend.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
    {
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder.HasKey(m => m.ApiKeyId);

            //builder.Property(m => m.ClientId).IsRequired();
            //builder.Property(m => m.ExpiresOn).IsRequired();
            //builder.Property(m => m.Key).IsRequired();

            builder.ToTable("api_key");
        }
    }
}
