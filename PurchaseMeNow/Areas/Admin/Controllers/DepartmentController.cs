
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Department department = new Department();

            //create
            if (id == null)
            {
                return View(department);
            }

            //edit
            department = _unitOfWork.Department.Get(id.GetValueOrDefault());

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Department department)
        {
            if (ModelState.IsValid)
            {
                if (department.Id == 0)
                {
                    _unitOfWork.Department.Add(department);
                }
                else
                {
                    _unitOfWork.Department.Update(department);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        #region API CAlls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allobj = _unitOfWork.Department.GetAll();
            return Json(new { data = allobj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Department.Get(id);

            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }
            _unitOfWork.Department.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }




        #endregion



    }

}