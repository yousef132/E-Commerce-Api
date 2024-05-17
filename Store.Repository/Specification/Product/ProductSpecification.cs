using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductSpecification
    {
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public const int MAXPAGESIZE = 50;
        public string? Sort {  get; set; }

        public int PageIndex { get; set; } = 1;

        private int pageSize = 6;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value < MAXPAGESIZE ?value : MAXPAGESIZE;
        }

        private string?  search;

        public string?  Search
        {
            get => search;
            set => search = value?.Trim().ToLower();
        }
    }
}
