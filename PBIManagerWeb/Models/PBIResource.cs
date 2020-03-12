using System.ComponentModel.DataAnnotations;

namespace PBIManagerWeb.Models
{
    public class PBIResource
    {
        public string Id { get; set; }
        //public string Name { get; set; }
        [Display(Name = "Running")]
        public bool IsRunning { get; set; }

        [Display(Name = "Name")]
        [Required, MinLength(3), MaxLength(500)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Display(Name = "Resource Group")]
        [Required]
        public string ResourceGroupId { get; set; }
        
        [Display(Name = "Sku")]
        [Required]
        public string SkuId { get; set; }
        
        [Display(Name = "Region")]
        [Required]
        public string RegionId { get; set; }

        [Display(Name = "Capacity Administrator")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Not a valid Email address")]
        public string CapacityAdministrator { get; set; }
    }
}
