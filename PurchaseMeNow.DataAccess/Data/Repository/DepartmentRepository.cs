using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PurchaseMeNow.DataAccess.Data.Repository
{
    public class DepartmentRepository: Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Department department)
        {
            var objFromDb = _db.Departments.FirstOrDefault(d => d.Id == department.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = department.Name;
            }
        }

   
    }
}
