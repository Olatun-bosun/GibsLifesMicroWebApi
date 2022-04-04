using System.Threading.Tasks;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository 
    {
        private readonly DataContext _db;

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
