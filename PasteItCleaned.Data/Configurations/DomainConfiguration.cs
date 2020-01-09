using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain>
    {
        public void Configure(EntityTypeBuilder<Domain> builder)
        {
            builder.HasKey(m => m.DomainId);

            builder.ToTable("domain");
        }
    }
}
