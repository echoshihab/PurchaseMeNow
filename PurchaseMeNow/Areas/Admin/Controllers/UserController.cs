using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
 
        }


      
        public IActionResult Index()
        {
            return View();
        }



  
        //public IActionResult Upsert(int? id)
        //{
        //    Category category = new Category();

        //    //create
        //    if(id == null)
        //    {
        //        return View(category);
        //    }

        //    //edit
        //    category = _unitOfWork.Category.Get(id.GetValueOrDefault());

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);

        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Category category)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if(category.Id == 0)
        //        {
        //            _unitOfWork.Category.Add(category);
                    
        //        }
        //        else
        //        {
        //            _unitOfWork.Category.Update(category);
        //        }

        //        _unitOfWork.Save();
        //        return RedirectToAction(nameof(Index));

        //    }

        //    return View(category);
        //}





        #region API CAlls 
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Department");
            var userRole = _unitOfWork.ApplicationRole.GetAll();
          

            return Json(new { data = userRole });
        }

       




        #endregion

    }
}