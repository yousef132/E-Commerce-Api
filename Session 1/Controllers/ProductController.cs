using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Session_1.Helper;
using Store.Data.Entities;
using Store.Repository.Specification.Product;
using Store.Service.HandleResponses;
using Store.Service.Helper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Session_1.Controllers
{

    public class ProductController : BaseController
	{
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        [Cache(30)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands()
            => Ok(await productService.GetAllBrandsAsync()); 

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
            => Ok(await productService.GetAllTypesAsync()); 


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts(
            [FromQuery]ProductSpecification productSpecification)

            => Ok(await productService.GetAllProductsAsync(productSpecification)); 
        
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? Id)
        {


            if(Id is null)
                return BadRequest(new Response(400,"Id Is Null"));

            var product = await productService.GetProductByIdAsync(Id);

            if(product == null)
               return NotFound(new Response(404));
            
            return Ok(product);
        }
    }
}
