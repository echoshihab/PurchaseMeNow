using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IDepartmentRepository Department { get; }
        IProductRepository Product { get; }

        IApplicationUserRepository ApplicationUser { get; }

        IOrderRepository Order { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IApprovalDecisionRepository ApprovalDecision { get; }
        IPurchaseDecisionRepository PurchaseDecision { get; }

        IDeliveryDetailRepository DeliveryDetail { get; }


        void Save();
    }
}
