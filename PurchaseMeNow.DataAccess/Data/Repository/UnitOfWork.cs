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
            
        }

        public ICategoryRepository Category { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IProductRepository Product { get; private set; }


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
