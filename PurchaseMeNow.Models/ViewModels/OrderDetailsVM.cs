using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public bool OrderingClient { get; set; } 

    }
}
