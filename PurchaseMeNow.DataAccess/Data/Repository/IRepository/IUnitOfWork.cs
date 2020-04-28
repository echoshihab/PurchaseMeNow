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
        ILocationRepository Location { get; }
        IApplicationUserRepository ApplicationUser { get; }

        IOrderRepository Order { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }

        
        void Save();
    }
}
