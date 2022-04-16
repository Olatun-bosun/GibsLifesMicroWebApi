using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository 
    {
        private readonly DataContext _db;
        private AuthContext _authContext;

        private const string BRANCH_ID = "19";
        private const string BRANCH_NAME = "RETAIL OFFICE";
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

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }











        public Task<List<Document>> DocumentSelectAsync(string policyNo)
        {
            var query = _db.Documents.Where(x => x.PolicyNo == policyNo);

            return query.ToListAsync();
        }

        public Task<Document> DocumentSelectThisAsync(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId))
                throw new ArgumentNullException(nameof(documentId));

            return _db.Documents.Where(x => x.DocumentId == documentId).SingleOrDefaultAsync();
        }

        public async Task<List<string>> DocumentCreateAsync(string policyNo, List<IFormFile> formFiles)
        {
            if (formFiles is null)
                throw new ArgumentNullException(nameof(formFiles));

            var policy = await PolicySelectThisAsync(policyNo);

            if (policy is null)
                throw new KeyNotFoundException("Policy No you supplied is invalid");



            //if (file == null || file.Length == 0)
            //    return Content("file not selected");

            //var path = Path.Combine(
            //    Directory.GetCurrentDirectory(), "uploads",
            //    file.FileName);

            //await file.CopyToAsync(stream);



            //var document = new Document
            //{
            //    SubmitDate = DateTime.Now,
            //    PolicyNo = policyNo,
            //    DocumentName = fileName,
            //    DocumentId = Guid.NewGuid().ToString(),
            //    Content = contents,
            //    //ContentType = "application/json",
            //};

            //_db.Documents.Add(document);
            //return document;
            return null;
        }

    }
}
