using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Models.ViewModels;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                DepartmentList = _unitOfWork.Department.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })

            };

            //create new
            if (Id == null)
            {
                return View(productVM);
            }

            //edit existing
            productVM.Product = _unitOfWork.Product.Get(Id.GetValueOrDefault()); ;

            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (product.Id == 0)
        //        {
        //            _unitOfWork.Product.Add(product);
        //        }
        //        else
        //        {
        //            _unitOfWork.Product.Update(product);
        //        }
        //        _unitOfWork.Save();
        //        RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}

        #region Api CAlls
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties:"Category,Department");

            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var objFromDb = _unitOfWork.Product.Get(Id);

            if (objFromDb == null)
            {
                return Json(new { sucess = false, message = "Error While Deleting" });
            }

            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }


        #endregion
    }

}