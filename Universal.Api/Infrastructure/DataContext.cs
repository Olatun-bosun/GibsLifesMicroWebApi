﻿using Microsoft.EntityFrameworkCore;
using GibsLifesMicroWebApi.Models;
using GibsLifesMicroWebApi.Domain;

namespace GibsLifesMicroWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PolicyAutoNumber>().HasKey(p => new { p.NumType, p.RiskID, p.BranchID, p.CompanyID });

            builder.Entity<PolicyDetail>()
                    .HasOne(pd => pd.Policy)
                    .WithMany(p => p.PolicyDetails)
                    .IsRequired(false);
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ApiUser> OpenApiUsers { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<SubRisks> SubRisks { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Claim> ClaimsReserved { get; set; }
        public DbSet<DNCNNote> DNCNNotes { get; set; }
        public DbSet<PolicyDetail> PolicyDetails { get; set; }
        public DbSet<InsuredClient> InsuredClients { get; set; }
        public DbSet<PolicyMaster> PolicyMaster { get; set; }
        public DbSet<Agents> Agents { get; set; }


    }
}
