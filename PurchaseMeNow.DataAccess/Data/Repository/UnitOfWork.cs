using Microsoft.AspNetCore.Identity;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Department = new DepartmentRepository(_db);
            Product = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Order = new OrderRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Location = new LocationRepository(_db);

        }

        public ICategoryRepository Category { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ILocationRepository Location { get; private set; }
        public IProductRepository Product { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderRepository Order { get; private set; }
        
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
       

        

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
