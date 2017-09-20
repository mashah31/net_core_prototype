using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using ABC.NetCore.Models;
using ABC.NetCore.CustomAttributes;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class PriorityCodes: Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableNumber]
        public int Priority { get; set; }

        [SearchableString]
        public string Description { get; set; }
    }

    public class PriorityCodesEntity
    {
        public Guid Id { get; set; }

        public int Priority { get; set; }

        public string Description { get; set; }
    }

    public class PriorityCodesRequest
    {
        [Required]
        [Display(Name = "Priority", Description = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Priority", Description = "Priority")]
        public string Description { get; set; }
    }

    public class PriorityCodesResponse: PagedCollection<PriorityCodes>
    {

    }
}
