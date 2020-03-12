using System.ComponentModel.DataAnnotations;

namespace PBIManagerWeb.Models
{
    public class PBIWorkspace
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required, MinLength(3), MaxLength(500)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Capacity Resource")]
        public string CapacityName { get; set; }
    }
}
