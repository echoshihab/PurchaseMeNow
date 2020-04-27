using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PurchaseMeNow.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Location")]
        [MaxLength(30)]
        public string Name { get; set; }
        
    }
}
