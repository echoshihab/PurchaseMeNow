using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models.ViewModels;

namespace PurchaseMeNow.Areas.Client.Controllers
{
    public class OrderController : Controller

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderListVM OrderListVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IEmailSender emailSender,  UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderListVM = new OrderListVM()
            {
                OrderHeader = new Models.OrderHeader(),
                OrderList = _unitOfWork.Order.GetAll(u => u.ApplicationUserId == claim.Value, 
                                                    includeProperties:"Product")
            };

            OrderListVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                                                        .GetFirstOrDefault(u => u.Id == claim.Value,
                                                         includeProperties: "Department");

            return View(OrderListVM);
        }
    }
}