using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductsWithSpecifications : BaseSpecification<Store.Data.Entities.Product>
    {
        // this class has the following:

        // applay specification in every ctor
        // get all
        public ProductsWithSpecifications(ProductSpecification specs)
            : base(
                  product => (product.BrandId == specs.BrandId || !specs.BrandId.HasValue) &&
                  (product.TypeId == specs.TypeId || !specs.TypeId.HasValue)  &&// where
                  (String.IsNullOrEmpty(specs.Search)|| product.Name.Trim().ToLower().Contains(specs.Search)) // search
                  )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderBy(x => x.Name);

            if (!String.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }

            ApplyPgination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);
        }
        // get by id
        public ProductsWithSpecifications(int? Id)
            : base(product => product.Id == Id) // where
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
    }
}
