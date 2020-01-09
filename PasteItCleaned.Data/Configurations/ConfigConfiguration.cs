using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Config>
    {
        public void Configure(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(m => m.ConfigId);

            builder.ToTable("config");
        }
    }
}
