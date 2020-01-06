using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasteItCleaned.Backend.Core.Models;

namespace PasteItCleaned.Backend.Data.Configurations
{
    public class HitConfiguration : IEntityTypeConfiguration<Hit>
    {
        public void Configure(EntityTypeBuilder<Hit> builder)
        {
            builder
                .HasKey(m => m.HitId);

            builder
                .Property(m => m.HitId);

            builder
                .Property(m => m.Hash)
                .IsRequired();

            /*builder
                .HasOne(m => m.Artist)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId);*/

            builder
                .ToTable("Hits");
        }
    }
}
