using System.Threading.Tasks;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository 
    {
        private readonly DataContext _db;
        private const string BRANCH_ID = "19";
        private const string BRANCH_NAME = "RETAIL OFFICE";
        private const string SUBMITTED_BY = "WEB-API";

        public Repository(DataContext db)
        {
            _db = db;
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }        
    }
}
