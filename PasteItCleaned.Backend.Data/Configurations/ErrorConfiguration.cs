using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Backend.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.HasKey(m => m.ErrorId);

            builder.ToTable("error");
        }
    }
}
