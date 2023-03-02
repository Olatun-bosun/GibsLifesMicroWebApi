using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository 
    {
        private readonly DataContext _db;
        private AuthContext _authContext;

        //private const string BRANCH_ID = "19";
        //private const string BRANCH_NAME = "RETAIL OFFICE";
        private const string SUBMITTED_BY = "WEB-API";

        public Repository(DataContext db, AuthContext sc)
        {
            _db = db;
            _authContext = sc;
        }

        public Task<ApiUser> AppLogin(string username, string password)
        {
           return _db.OpenApiUsers
                     .Where(x => x.CompanyName == username
                              && x.Password    == password).SingleOrDefaultAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
