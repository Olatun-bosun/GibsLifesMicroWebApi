using Microsoft.EntityFrameworkCore;
using Universal.Api.Models;

namespace Universal.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<SubRisk> SubRisks { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Claim> ClaimsReserved { get; set; }
        public DbSet<DNCNNote> DNCNNotes { get; set; }
        public DbSet<PolicyDetail> PolicyDetails { get; set; }
        public DbSet<InsuredClient> InsuredClients { get; set; }
    }
}
