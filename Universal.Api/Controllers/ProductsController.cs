using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;
using Microsoft.AspNetCore.Authorization;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : SecureControllerBase
    {
        public ProductsController(IRepository repository) : base(repository)
        {
        }


        /// <summary>
        /// Fetch a collection of products.
        /// </summary>
        /// <returns>A collection of products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> ListProductsAsync(
            [FromQuery] string searchText, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var subrisks = await _repository.SubRisksSelectAsync(searchText, pageNo, pageSize);
                return Ok(subrisks.Select(sr => new ProductDto(sr)).ToList());
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// Fetch a single product.
        /// </summary>
        /// <param name="subriskId">Id of the product to get.</param>
        /// <returns>The product with the subrisk Id</returns>
        [HttpGet("{subriskId}")]
        public ActionResult<ProductDto> GetProduct(string subriskId)
        {
            try
            {
                var subrisk = _repository.SubRiskSelectThis(subriskId);
                if (subrisk is null)
                {
                    return NotFound();
                }

                return Ok(new ProductDto(subrisk));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
