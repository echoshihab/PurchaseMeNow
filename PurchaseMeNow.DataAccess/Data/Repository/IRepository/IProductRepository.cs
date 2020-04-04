using PurchaseMeNow.Models;

namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface IProductRepository: IRepository<Product>
    {
        void Update(Product product);
    }
}
