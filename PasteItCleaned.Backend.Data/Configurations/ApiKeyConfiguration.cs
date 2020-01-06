using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Backend.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
    {
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder
                .HasKey(m => m.ApiKeyId);

            builder
                .Property(m => m.ApiKeyId);

            builder
                .Property(m => m.Key)
                .IsRequired()
                .HasMaxLength(50);

            /*builder
                .HasOne(m => m.Artist)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId);*/

            builder
                .ToTable("ApiKeys");
        }
    }
}
