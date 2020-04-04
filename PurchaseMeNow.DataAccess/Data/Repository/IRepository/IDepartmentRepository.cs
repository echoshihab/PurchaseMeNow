using PurchaseMeNow.Models;


namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        void Update(Department department);
    }
}
