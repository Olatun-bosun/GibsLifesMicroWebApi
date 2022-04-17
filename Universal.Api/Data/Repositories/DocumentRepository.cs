using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {

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

            var documentIds = new List<string>();

            foreach (var file in formFiles)
            {
                if (file == null || file.Length == 0)
                    throw new Exception("Error in uploaded file, empty content");

                var document = new Document
                {
                    PolicyNo = policyNo,
                    SubmitDate = DateTime.Now,
                    DocumentName = file.FileName,
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = file.OpenReadStream().ReadFully(),
                    //ContentType = "application/json",
                };

                documentIds.Add(document.DocumentId);
                _db.Documents.Add(document);
            }

            return documentIds;
        }
    }
}
