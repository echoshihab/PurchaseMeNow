using PurchaseMeNow.Models;


namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface IOrderRepository: IRepository<Order>
    {
        void Update(Order order);
    }
}
