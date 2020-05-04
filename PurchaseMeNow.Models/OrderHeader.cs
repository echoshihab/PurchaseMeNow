using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PurchaseMeNow.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]

        public ApplicationUser ApplicationUser { get; set; }

        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]

        public Location Location { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public DateTime ShippingDate { get; set; }


    }
}
