using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PurchaseMeNow.Models.ViewModels
{
    public class OrderListVM
    {
        public IEnumerable<Order> OrderList { get; set; }
        [Required]
        [Display(Name = "Shipping Location")]
        public int SelectedLocationId { get; set; }
        public IEnumerable<SelectListItem> LocationList {get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
