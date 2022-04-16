using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;
using Universal.Api.Data;

namespace Universal.Api.Controllers
{
    [Authorize(Roles = "APP,AGENT,CUST")]
    public class DocumentsController : SecureControllerBase
    {
        public DocumentsController(Repository repository, AuthContext authContext) : base(repository, authContext)
        {
        }

        /// <summary>
        /// Fetch a collection of Documents for a Policy
        /// </summary>
        /// <param name="policyNo"></param>
        /// <returns>A collection of Documents</returns>
        [HttpGet("{policyNo}")]
        public async Task<ActionResult<IEnumerable<FileContentResult>>> ListDocuments(string policyNo)
        {
            try
            {
                var documents = await _repository.DocumentSelectAsync(policyNo);
                return documents.Select(x => new FileContentResult(x.Content, "application/octet-stream")).ToList();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single Document
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns>The Document with the DocumentId supplied</returns>
        [HttpGet("{documentId}")]
        public async Task<ActionResult> GetDocument(string documentId)
        {
            try
            {
                var document = await _repository.DocumentSelectThisAsync(documentId);

                if (document is null)
                    return NotFound();

                return new FileContentResult(document.Content, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Upload Documents and associate with a Policy
        /// </summary>
        /// <returns>The newly created Documents IDs</returns>
        [HttpPost("{policyNo}")]
        public async Task<ActionResult<IEnumerable<string>>> UploadDocuments(List<IFormFile> formFiles, string policyNo)
        {
            try
            {
                var documentIds = await _repository.DocumentCreateAsync(policyNo, formFiles);

                return Ok(documentIds);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
