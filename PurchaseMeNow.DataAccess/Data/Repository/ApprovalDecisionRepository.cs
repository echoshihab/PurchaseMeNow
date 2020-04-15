using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class ApprovalDecisionRepository: Repository<ApprovalDecision>, IApprovalDecisionRepository
    {
        private readonly ApplicationDbContext _db;

        public ApprovalDecisionRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }


    }
}
