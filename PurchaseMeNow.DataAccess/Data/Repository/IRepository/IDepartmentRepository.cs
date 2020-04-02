using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        void Update(Department department);
    }
}
