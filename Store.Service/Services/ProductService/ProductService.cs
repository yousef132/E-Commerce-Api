using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Product;
using Store.Service.Helper;
using Store.Service.Services.ProductService.Dtos;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.Reposirory<ProductBrand, int>().GetAllAsync();

            var mappedBrands = mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);

            return mappedBrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification productSpecification)
        {
            var specs = new ProductsWithSpecifications(productSpecification);

            var products = await unitOfWork.Reposirory<Product, int>().GetWithSpecificationsAllAsync(specs);

            var countspecs = new ProductsWithFilterCountSpecifications(productSpecification);

            var count =  await unitOfWork.Reposirory<Product,int>().CountSpecificationAsync(countspecs);

            var mappedProducts = mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);

            return new PaginatedResultDto<ProductDetailsDto>(productSpecification.PageIndex, productSpecification.PageSize, count, mappedProducts);
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
           var types = await unitOfWork.Reposirory<ProductType, int>().GetAllAsync();
           var mappedTypes = mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(types);
            return mappedTypes;
        }
        public async Task<ProductDetailsDto> GetProductByIdAsync(int? Id)
        {
            if(Id is null) 
                throw new Exception("Id Is Null");

            // My Specifications
            var spec = new ProductsWithSpecifications(Id);

            Product product = await unitOfWork.Reposirory<Product, int>().GetWithSpecificationsByIdAsync(spec);
            if(product is null)
                throw new Exception("Product Not Found");
            var mappedProduct = mapper.Map<ProductDetailsDto>(product);
            return mappedProduct;
        }

    }
}