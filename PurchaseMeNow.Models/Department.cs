using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PurchaseMeNow.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Display (Name="Department")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

    }
}
