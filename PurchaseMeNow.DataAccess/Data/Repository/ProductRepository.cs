

using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System.Linq;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == product.Id);

            if(objFromDb != null)
            {
                objFromDb.Name = product.Name;
                objFromDb.Description = product.Description;
                objFromDb.DepartmentId = product.DepartmentId;
                objFromDb.CategoryId = product.CategoryId;
            }
        }
    }
}
