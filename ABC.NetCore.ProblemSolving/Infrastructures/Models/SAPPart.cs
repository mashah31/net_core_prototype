using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ABC.NetCore.CustomAttributes;
using ABC.NetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class SAPPart: Resource
    {
        [Sortable]
        [SearchableString]
        public string SAPMaterialNum { get; set; }

        [Sortable]
        [SearchableString]
        public string CompanyNumber { get; set; }
        
        [Sortable(Default = true)]
        [SearchableString]
        public string PartDesc { get; set; }
    }

    public class SAPPartEntity
    {
        [Key]
        public string SAPMaterialNum { get; set; }

        public string CompanyNumber { get; set; }

        public string PartDesc { get; set; }
    }

    public class SAPPartResponse: PagedCollection<SAPPart>
    {
        
    }
}
