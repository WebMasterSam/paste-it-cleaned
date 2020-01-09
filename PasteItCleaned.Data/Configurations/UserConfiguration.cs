using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(m => m.UserId);

            builder.ToTable("user");
        }
    }
}
