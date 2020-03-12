using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBIManagerWeb.Models
{
    public class PBICapacity
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required, MinLength(3), MaxLength(500)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Display(Name = "Resource Group")]
        [Required]
        public string ResourceGroup { get; set; }
        
        [Display(Name = "Sku")]
        [Required]
        public string Sku { get; set; }
        
        [Display(Name = "Region")]
        [Required]
        public string Region { get; set; }

        [Display(Name = "Status")]
        [Required]
        public string Status { get; set; }

        [Display(Name = "Capacity Administrators")]
        [Required]
        public IList<string> Administrators { get; set; }
    }
}
