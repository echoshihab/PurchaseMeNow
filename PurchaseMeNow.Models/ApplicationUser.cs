using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PurchaseMeNow.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
