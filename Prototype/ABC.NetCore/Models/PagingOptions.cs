using System;
using System.ComponentModel.DataAnnotations;

namespace ABC.NetCore.Models
{
    public class PagingOptions
    {
        [Range(0, Double.MaxValue, ErrorMessage = "Offset must be greter than 0 and less than 9999999999")]
        public int? Offset { get; set; }

        [Range(0, 100, ErrorMessage = "Limit must be greter than 1 and less than 100")]
        public int? Limit { get; set; }
    }
}
