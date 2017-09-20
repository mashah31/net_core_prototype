using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABC.NetCore.Models;
using ABC.NetCore.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class ProblemSolvingType : Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableString]
        public string Tag { get; set; }

        [Sortable]
        [SearchableString]
        public string SAPCode { get; set; }

        [Sortable]
        [SearchableString]
        public string SAPSubCode { get; set; }

        [Sortable]
        [SearchableString]
        public string Use { get; set; }
    }

    public class ProblemSolvingTypeEntity
    {
        public Guid Id { get; set; }

        public string Tag { get; set; }

        public string SAPCode { get; set; }

        public string SAPSubCode { get; set; }

        public string Use { get; set; }
    }

    public class ProblemSolvingTypeRequest
    {
        [Required]
        [Display(Name = "Problem Solving Type Tag", Description = "Tag of problem solving type.")]
        public string Tag { get; set; }

        [Display(Name = "SAP Code", Description = "SAP Code")]
        public string SAPCode { get; set; }

        [Display(Name = "SAP SubCode", Description = "SAP SubCode")]
        public string SAPSubCode { get; set; }

        [Display(Name = "Use of Problem Solving Type", Description = "Use of Problem Solving Type")]
        public string Use { get; set; }
    }

    public class ProblemSolvingTypeResponse : PagedCollection<ProblemSolvingType>
    {

    }
}
