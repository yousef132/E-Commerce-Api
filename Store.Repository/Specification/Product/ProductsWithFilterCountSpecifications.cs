using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductsWithFilterCountSpecifications : BaseSpecification<Store.Data.Entities.Product>
    {
        public ProductsWithFilterCountSpecifications(ProductSpecification specs)
            : base(
                  product => (product.BrandId == specs.BrandId || !specs.BrandId.HasValue) &&
                  (product.TypeId == specs.TypeId || !specs.TypeId.HasValue)  &&// where
                  (String.IsNullOrEmpty(specs.Search)|| product.Name.Trim().ToLower().Contains(specs.Search)) // search
                  )
        {

        }
    }
}
