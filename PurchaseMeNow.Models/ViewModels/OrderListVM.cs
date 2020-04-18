using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.Models.ViewModels
{
    public class OrderListVM
    {
        public IEnumerable<Order> OrderList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
