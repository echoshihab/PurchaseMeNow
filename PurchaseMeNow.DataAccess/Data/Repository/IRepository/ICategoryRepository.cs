﻿using PurchaseMeNow.Models;

namespace PurchaseMeNow.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
