using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;

namespace Universal.Api.Controllers
{
    public class ProductsController : SecureControllerBase
    {
        public ProductsController(Repository repository) : base(repository)
        {
        }


        /// <summary>
        /// Fetch a collection of products.
        /// </summary>
        /// <returns>A collection of products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResult>>> ListProductsAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                var subrisks = await _repository.ProductSelectAsync(filter);
                return Ok(subrisks.Select(x => new ProductResult(x)).ToList());
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// Fetch a single product.
        /// </summary>
        /// <param name="productId">Id of the product to get.</param>
        /// <returns>The product with the subrisk Id</returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductResult>> GetProduct(string productId)
        {
            try
            {
                var product = await _repository.ProductSelectThisAsync(productId);

                if (product is null)
                    return NotFound();

                return Ok(new ProductResult(product));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
