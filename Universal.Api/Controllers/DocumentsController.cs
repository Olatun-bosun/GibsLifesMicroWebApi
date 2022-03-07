using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : SecureControllerBase
    {
        public DocumentsController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Fetch a collection of documents.
        /// </summary>
        /// <returns>A collection of documents.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> ListDocumentsAsync(
            [FromQuery] string searchText, [FromQuery] int skipCount, [FromQuery] int pageSize)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single document.
        /// </summary>
        /// <param name="documentId">Id of the document to get.</param>
        /// <returns>The document with the Id entered.</returns>
        [HttpGet("{documentId}")]
        public ActionResult<DocumentDto> GetDocument(string documentId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create a document.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Documents
        ///     {
        ///        "name": "string",
        ///         "remarks": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="documentDetails"></param>
        /// <returns>A newly created Document</returns>
        [HttpPost]
        public ActionResult<DocumentDto> Post(DocumentDto documentDetails)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
