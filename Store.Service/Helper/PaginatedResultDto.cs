using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper
{
    public class PaginatedResultDto<T>
    {
        public PaginatedResultDto(int pageIndex, int count, int pageSize, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            Count = count;
            PageSize = pageSize;
            Data = data;
        }

        public int PageIndex { get; set; }  
        public int Count { get; set; }

        public int PageSize { get; set; }   

        public IReadOnlyList<T> Data { get; set; }   

    }
}
