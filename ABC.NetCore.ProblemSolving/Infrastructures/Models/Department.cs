using ABC.NetCore.CustomAttributes;
using ABC.NetCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class Department : Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableString]
        public string Tag { get; set; }

        [SearchableString]
        public string Description { get; set; }
    }

    public class DepartmentEntity
    {
        public Guid Id { get; set; }

        public string Tag { get; set; }

        public string Description { get; set; }
    }

    public class DepartmentRequest
    {
        [Required]
        [Display(Name = "Department", Description = "Department")]
        public string Tag { get; set; }

        [Display(Name = "Description", Description = "Description")]
        public string Description { get; set; }
    }

    public class DepartmentResponce : PagedCollection<Department>
    {

    }
}
