using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PBIManagerWeb.Models
{
    public class PBISku
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Capacity { get; set; }
    }

    //public enum PBISKUName
    //{
    //    [Display(Name = "A1")]
    //    A1,
    //    [Display(Name = "A2")]
    //    A2,
    //    [Display(Name = "A3")]
    //    A3,
    //    [Display(Name = "A4")]
    //    A4,
    //    [Display(Name = "A5")]
    //    A5,
    //    [Display(Name = "A6")]
    //    A6
    //}

    //public enum PBISKUTier
    //{
    //    [Display(Name = "Standard")]
    //    Standard,
    //    [Display(Name = "PBIE_Azure")]
    //    PBIE_Azure
    //}
}