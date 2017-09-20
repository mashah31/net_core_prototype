using ABC.NetCore.CustomAttributes;
using ABC.NetCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class SAPEmployee: Resource
    {
        [Sortable]
        [SearchableString]
        public string UserName { get; set; }

        [Sortable]
        [SearchableString]
        public string LastName { get; set; }
        
        [Sortable(Default = true)]
        [SearchableString]
        public string FirstName { get; set; }

        [Sortable]
        [SearchableString]
        public string CostCenter { get; set; }

        [Sortable]
        [SearchableString]
        public string OrgUnitNumber { get; set; }

        [Sortable]
        [SearchableString]
        public string Department { get; set; }
    }

    public class SAPEmployeeEntity
    {
        [Key]
        public string ClockNumber { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string BirthDate { get; set; }

        public string CostCenter { get; set; }

        public string OrgUnitNumber { get; set; }

        public string Department { get; set; }

        public int? Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdateCheck { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
    }

    public class SAPEmployeeResponse: PagedCollection<SAPEmployee>
    {

    }
}
