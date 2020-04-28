using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Utility;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


      
        public IActionResult Index()
        {
            return View();
        }



  
        public IActionResult Upsert(int? id)
        {
            Location location = new Location();

            //create
            if(id == null)
            {
                return View(location);
            }

            //edit
            location = _unitOfWork.Location.Get(id.GetValueOrDefault());

            if (location == null)
            {
                return NotFound();
            }

            return View(location);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Location location)
        {
            if(ModelState.IsValid)
            {
                if(location.Id == 0)
                {
                    _unitOfWork.Location.Add(location);
                    
                }
                else
                {
                    _unitOfWork.Location.Update(location);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }

            return View(location);
        }





        #region API CAlls 
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Location.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Location.Get(id);

            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }
            _unitOfWork.Location.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }




        #endregion

    }
}