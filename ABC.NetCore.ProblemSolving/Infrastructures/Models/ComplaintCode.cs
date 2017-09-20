using ABC.NetCore.CustomAttributes;
using ABC.NetCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class ComplaintCode: Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableString]
        public string GroupCode { get; set; }

        [Sortable]
        [SearchableString]
        public string GroupCodeDescription { get; set; }

        [Sortable]
        [SearchableString]
        public string Code { get; set; }

        [Sortable]
        [SearchableString]
        public string CodeDescription { get; set; }

        [Sortable]
        [SearchableDateTime]
        public DateTime Created { get; set; }

        [Sortable]
        [SearchableString]
        public string CreatedBy { get; set; }

        [Sortable]
        [SearchableDateTime]
        public DateTime Modified { get; set; }

        [Sortable]
        [SearchableString]
        public string ModifiedBy { get; set; }
    }

    public class ComplaintCodeEntity
    {
        public Guid Id { get; set; }

        public string GroupCode { get; set; }

        public string GroupCodeDescription { get; set; }

        public string Code { get; set; }

        public string CodeDescription { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class ComplaintCodeRequest
    {
        [Required]
        [Display(Name = "Complaint Group Code", Description = "Complaint group code should be number.")]
        public int GroupCode { get; set; }

        [Required]
        [Display(Name = "Complaint Group Code Description", Description = "Description of complaint group code.")]
        public string GroupCodeDescription { get; set; }

        [Required]
        [Display(Name = "Complaint Code", Description = "Complaint code should be number.")]
        public int Code { get; set; }

        [Required]
        [Display(Name = "Complaint Code Description", Description = "Description of complaint code.")]
        public string CodeDescription { get; set; }
                
        [Display(Name = "Created at", Description = "Creation date of complaint code.")]
        public DateTime Created { get; set; }

        [Display(Name = "Created By", Description = "Username of person who have created complaint code.")]
        public string CreatedBy { get; set; }
                
        [Display(Name = "Modified at", Description = "Modified date for complaint code.")]
        public DateTime Modified { get; set; }

        [Display(Name = "Modified By", Description = "Username of person who have modified complaint code.")]
        public string ModifiedBy { get; set; }
    }

    public class ComplaintCodeResponce: PagedCollection<ComplaintCode>
    {

    }
}
