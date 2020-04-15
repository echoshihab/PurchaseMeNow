using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class PurchaseDecisionRepository : Repository<PurchaseDecision>, IPurchaseDecisionRepository
    {
        private readonly ApplicationDbContext _db;

        public PurchaseDecisionRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
    }
}
