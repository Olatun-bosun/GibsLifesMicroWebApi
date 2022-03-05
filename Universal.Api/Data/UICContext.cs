using Microsoft.EntityFrameworkCore;
using Universal.Api.Models;

namespace Universal.Api.Data
{
    public class UICContext : DbContext
    {
        public UICContext(DbContextOptions<UICContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthUser>().ToTable("Webservice_ID");
        }

        public DbSet<AuthUser> AuthUsers{ get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<InsuredClient> InsuredClients { get; set; }
        public DbSet<Claim> ClaimsReserved { get; set; }
        public virtual DbSet<DNCNNote> DNCNNotes { get; set; }
        public DbSet<SubRisk> SubRisks { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public virtual DbSet<PolicyDetail> PolicyDetails { get; set; }
    }
}
