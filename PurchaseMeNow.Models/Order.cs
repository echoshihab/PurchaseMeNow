﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PurchaseMeNow.Models
{
    public class Order
    {

        public Order()
        {
            Count = 1;
        }
        [Key]
        public int Id { get; set;}

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]

        public ApplicationUser ApplicationUser { get; set;}

        public int ProductId { get; set; } 

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(1,1000, ErrorMessage ="Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public DateTime OrderDate { get; set; }
        public bool? Approved { get; set; }
        public bool? Purchased { get; set; }

        public bool? Delivered { get; set; }
    }
}
