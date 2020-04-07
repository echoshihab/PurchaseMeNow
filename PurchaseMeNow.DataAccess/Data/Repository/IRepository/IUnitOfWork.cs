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

        IApplicationRoleRepository ApplicationRole { get; }

        void Save();
    }
}
