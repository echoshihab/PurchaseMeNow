using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class DeliveryDetailRepository: Repository<DeliveryDetail>, IDeliveryDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public DeliveryDetailRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
    }
}
