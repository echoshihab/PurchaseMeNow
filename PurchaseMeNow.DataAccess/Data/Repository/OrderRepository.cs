using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(Order order)
        {
            var objFromDb = _db.Orders.FirstOrDefault(o => o.Id == order.Id);

            if (objFromDb != null)
            {
                objFromDb.Approved = order.Approved;
                objFromDb.Purchased = order.Purchased;
                objFromDb.Approved = order.Approved;
            }
        }
    }
}
