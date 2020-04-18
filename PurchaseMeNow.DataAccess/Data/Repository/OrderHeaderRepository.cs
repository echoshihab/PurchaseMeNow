using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class OrderHeaderRepository: Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
    
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }
    }
}
