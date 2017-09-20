using System;
using System.Collections.Generic;
using System.Text;

namespace ABC.NetCore.Models
{
    public class Collection<T> : Resource
    {
        public T[] Values { get; set; }
    }
}
