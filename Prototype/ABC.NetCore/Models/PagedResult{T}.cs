using System;
using System.Collections.Generic;
using System.Text;

namespace ABC.NetCore.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Size { get; set; }
    }
}
