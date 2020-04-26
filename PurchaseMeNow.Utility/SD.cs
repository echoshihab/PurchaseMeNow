using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.Utility
{
   public static class SD
    {
        //roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        public const string Role_Coordinator = "Coordinator";

        //session details
        public const string ssOrder = "Order Session";

        //order status
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusInProcess = "Processing";
        public const string OrderStatusDelivered = "Delivered";
        public const string OrderStatusCancelled = "Rejected";
        public const string OrderStatusRejected = "Rejected";
        public const string OrderStatusShipped = "Shipped";


    }
}
