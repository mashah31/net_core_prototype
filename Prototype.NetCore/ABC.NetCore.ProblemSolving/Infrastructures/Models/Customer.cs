using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using ABC.NetCore.Models;
using ABC.NetCore.CustomAttributes;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class Customer: Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableString]
        public string Name { get; set; }

        [Sortable]
        [SearchableString]
        public string Location { get; set; }

        // TODO:: HETEOS REST Approach
        // Bellow link can be used to attach helper form object to each object for HETEOS approach with REST API
        // Commented out as it adds too much complexity with every REST API Response.
        // public Form Create { get; set; }
    }

    public class CustomerEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }

    public class CustomerRequest
    {
        [Required]
        [Display(Name = "Customer Name", Description = "Name of customer.")]
        public string Name { get; set; }

        [Display(Name = "Customer Location", Description = "Location of customer.")]
        public string Location { get; set; }
    }

    public class CustomerResponse : PagedCollection<Customer>
    {
        // TODO:: HETEOS REST Approach
        // Bellow link can be used to attach helper form object to each object for HETEOS approach with REST API
        // Commented out as it adds too much complexity with every REST API Response.
        // public Form CustomerQuery { get; set; }
    }
}
