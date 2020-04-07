using Microsoft.AspNetCore.Identity;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class ApplicationRoleRepository : Repository<IdentityRole>, IApplicationRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


    }
}
