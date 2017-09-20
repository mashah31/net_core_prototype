using ABC.NetCore.CustomAttributes;
using ABC.NetCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.NetCore.ProblemSolving.Models
{
    public class Plant: Resource
    {
        public Guid Id { get; set; }

        [Sortable(Default = true)]
        [SearchableString]
        public string Tag { get; set; }

        [SearchableString]
        public string Description { get; set; }

        [SearchableString]
        public string IdSAP { get; set; }

        [Sortable]
        [SearchableString]
        public string Location { get; set; }
        
        [Sortable]
        [SearchableString]
        public string Region { get; set; }
    }

    public class PlantEntity
    {
        public Guid Id { get; set; }

        public string Tag { get; set; }

        public string Description { get; set; }

        public string IdSAP { get; set; }

        public string Location { get; set; }
                
        public string Region { get; set; }
    }

    public class PlantRequest
    {
        [Required]
        [Display(Name = "Plant Tag", Description = "Plant Tag")]
        public string Tag { get; set; }

        [Display(Name = "Description", Description = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "SAP ID", Description = "ID used in SAP; For example Fort Mill 1 = 0051")]
        public string IdSAP { get; set; }

        [Required]
        [Display(Name = "Location", Description = "Location of plant; For example Fort Mill 1 = IBC")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Region", Description = "Region of plant; For example Nort America = NA")]
        public string Region { get; set; }
    }

    public class PlantResponse: PagedCollection<Plant>
    {

    }
}
