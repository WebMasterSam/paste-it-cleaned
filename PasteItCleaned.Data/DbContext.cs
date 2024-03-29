﻿using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Data.Configurations;

namespace PasteItCleaned.Backend.Data
{
    public class PasteItCleanedDbContext : DbContext
    {
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Hit> Hits { get; set; }
        public DbSet<HitDaily> HitsDaily { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<User> Users { get; set; }

        public PasteItCleanedDbContext(DbContextOptions<PasteItCleanedDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApiKeyConfiguration());
            builder.ApplyConfiguration(new ClientConfiguration());
            builder.ApplyConfiguration(new ConfigConfiguration());
            builder.ApplyConfiguration(new DomainConfiguration());
            builder.ApplyConfiguration(new ErrorConfiguration());
            builder.ApplyConfiguration(new HitConfiguration());
            builder.ApplyConfiguration(new HitDailyConfiguration());
            builder.ApplyConfiguration(new InvoiceConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new PaymentMethodConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
