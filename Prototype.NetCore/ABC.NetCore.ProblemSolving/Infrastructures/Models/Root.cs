using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ABC.NetCore.Models;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class Root: Resource
    {
        public Link Customers { get; set; }
        public Link SAPParts { get; set; }
        public Link SAPEmployees { get; set; }
    }
}
