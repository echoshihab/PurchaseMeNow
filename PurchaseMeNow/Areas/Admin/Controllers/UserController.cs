using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PurchaseMeNow.DataAccess.Data;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Models.ViewModels;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;

        }


      
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id, includeProperties: "Department");

            if (user == null)
            {
                return NotFound();
            }
            user.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            ApplicationUserVM applicationUserVM = new ApplicationUserVM()
            {
                ApplicationUser = user,
                RoleList = _roleManager.Roles.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Name
                }),
                DepartmentList = _unitOfWork.Department.GetAll().Select(d => new SelectListItem
                {

                    Text = d.Name,
                    Value = d.Id.ToString(),
                    Selected = (d.Name == user.Department.Name ? true : false)

                 })

            };

            return View(applicationUserVM);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
           public IActionResult Edit(ApplicationUserVM applicationUserVM)
            
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == applicationUserVM.ApplicationUser.Id);

                if (userToUpdate != null)
                {
                    userToUpdate.Name = applicationUserVM.ApplicationUser.Name;
                    userToUpdate.Email = applicationUserVM.ApplicationUser.Email;
                    userToUpdate.DepartmentId = applicationUserVM.ApplicationUser.DepartmentId;
                }

                var result = _userManager.UpdateAsync(userToUpdate).Result;


                if (result.Succeeded)
                {
                    //try adding to role if user update successful
                    if (_roleManager.RoleExistsAsync(applicationUserVM.ApplicationUser.Role).Result)
                    {
                        //only update role if changed
                        if (!_userManager.IsInRoleAsync(userToUpdate, applicationUserVM.ApplicationUser.Role).Result)
                        {
                            var currentRole = _userManager.GetRolesAsync(userToUpdate).Result;
                            _userManager.RemoveFromRolesAsync(userToUpdate, currentRole).Wait();
                            _userManager.AddToRoleAsync(userToUpdate, applicationUserVM.ApplicationUser.Role).Wait();
                        }

                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error updating user");
                }



            }
            //return to edit page with proper viewmodel if there are issues.
            applicationUserVM.RoleList = _roleManager.Roles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name,
                Selected = (c.Name == applicationUserVM.ApplicationUser.Role ? true : false)
            });
            applicationUserVM.DepartmentList = _unitOfWork.Department.GetAll().Select(d => new SelectListItem
            {

                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = (d.Id == applicationUserVM.ApplicationUser.DepartmentId ? true : false)

            });


            return View(applicationUserVM);




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
          
            foreach(var user in userList)
            {
                user.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            }



            return Json(new { data = userList });
        }

       
        [HttpPost]
        public IActionResult ToggleLock([FromBody] string id)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id); 
            if (user == null)
            {
                return Json(new { success = false, message = "Error while toggling lock on user" });
            }

            if(user.LockoutEnd!=null && user.LockoutEnd > DateTime.Now)
            {
                //unlock user by setting lockout end time to now
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(200);
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Task Complete" });
        }



        #endregion

    }
}