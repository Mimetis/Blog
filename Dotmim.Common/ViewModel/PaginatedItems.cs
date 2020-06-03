using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Common.ViewModel
{
    public class PagedItems<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public IEnumerable<T> Data { get; set; }

        public PagedItems(int pageIndex, int pageSize, long count, IEnumerable<T> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }

        public PagedItems()
        {
            this.PageIndex = 0;
            this.PageSize = 1;
            this.Count = 0;
            this.Data = null;
        }
    }
}
