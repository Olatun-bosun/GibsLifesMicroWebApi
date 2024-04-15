using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GibsLifesMicroWebApi.Contracts.V1;
using GibsLifesMicroWebApi.Models;

namespace GibsLifesMicroWebApi.Data.Repositories
{
    public partial class Repository 
    {
        private readonly DataContext _db;
        private AuthContext _authContext;

        private const string BRANCH_ID = "19";
        //private const string BRANCH_NAME = "RETAIL OFFICE";
        private const string SUBMITTED_BY = "WEB-API";

        public Repository(DataContext db, AuthContext sc)
        {
            _db = db;
            _authContext = sc;
        }

        public Task<ApiUser> AppLogin(string username, string password)
        {
           return _db.OpenApiUsers.FirstOrDefaultAsync(x => x.CompanyName == username && x.Password == password);
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

        public AuditLog AuditLogCreateAsync(string logType, string source, string categories, string logDetails)
        {        
            var log = new AuditLog()
            {
                LogType = logType,
                Source = source,
                Category = categories,
                Description = logDetails,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
            };

            _db.AuditLogs.Add(log);
            return log;
        }
    }
}
