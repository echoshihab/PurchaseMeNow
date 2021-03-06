﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Models.ViewModels;
using PurchaseMeNow.Utility;

namespace PurchaseMeNow.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        [BindProperty]
        public OrderDetailsVM OrderDetailsVM { get; set; }

        public OrderController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsVM = new OrderDetailsVM()
            {
                OrderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(u => u.Id == id,
                includeProperties: "ApplicationUser,Location"),
                OrderDetails = _unitofWork.OrderDetail.GetAll(u => u.OrderHeaderId == id, includeProperties: "Product"),
                OrderingClient = false
            };

            //if the user viewing detail is the person who ordered it- will use this for confirm delivery permission
            if(OrderDetailsVM.OrderHeader.ApplicationUserId == claim.Value)
            {
                OrderDetailsVM.OrderingClient = true;
            }
            

        return View(OrderDetailsVM);
        }

        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Coordinator)]
        public IActionResult StartProcessing(int id)
        {
            OrderHeader OrderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            OrderHeader.OrderStatus = SD.OrderStatusInProcess;
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult ShipOrder()
        {
            OrderHeader OrderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderDetailsVM.OrderHeader.Id);
            OrderHeader.OrderStatus = SD.OrderStatusShipped;
            OrderHeader.ShippingDate = DateTime.Now;
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult ConfirmDelivery(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            OrderHeader OrderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);

            
            if (!User.IsInRole(SD.Role_Admin) && !User.IsInRole(SD.Role_Coordinator) && claim.Value != OrderHeader.ApplicationUserId)
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            OrderHeader.OrderStatus = SD.OrderStatusDelivered;
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader OrderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            OrderHeader.OrderStatus = SD.OrderStatusCancelled;
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }


        #region API Calls
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaderList;

            if(User.IsInRole(SD.Role_Admin))
            {
                orderHeaderList = _unitofWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else if(User.IsInRole(SD.Role_Coordinator))
            {
                var coordinatorDeptId = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).DepartmentId;
                orderHeaderList = _unitofWork.OrderHeader.GetAll(u=>u.ApplicationUser.DepartmentId==coordinatorDeptId, includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderList = _unitofWork.OrderHeader.GetAll(u=> u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusPending);
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusInProcess);
                    break;
                case "rejected":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusRejected 
                                                        ||  o.OrderStatus ==SD.OrderStatusCancelled);
                    break;
                case "shipped":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusShipped);
                    break;
                case "delivered":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusDelivered);
                    break;
                default:
                    break;

            }

            return Json(new { data = orderHeaderList });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetOrderCount()
        {
            int pendingCount=0, inProcessCount = 0, shippedCount = 0, rejectedCount = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Role_Admin))
            {
                pendingCount = _unitofWork.OrderHeader.GetAll().Where(o => o.OrderStatus == SD.OrderStatusPending).Count();
                inProcessCount = _unitofWork.OrderHeader.GetAll().Where(o => o.OrderStatus == SD.OrderStatusInProcess).Count();
                shippedCount = _unitofWork.OrderHeader.GetAll().Where(o => o.OrderStatus == SD.OrderStatusShipped).Count();
                rejectedCount = _unitofWork.OrderHeader.GetAll().Where(o => o.OrderStatus == SD.OrderStatusRejected).Count();

            }
            else if (User.IsInRole(SD.Role_Coordinator))
            {
                

                var coordinatorDeptId = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).DepartmentId;
                var orderHeaderList = _unitofWork.OrderHeader.GetAll(u => u.ApplicationUser.DepartmentId == coordinatorDeptId);
                pendingCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusPending).Count();
                inProcessCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusInProcess).Count();
                shippedCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusShipped).Count();
                rejectedCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusRejected).Count();
            }

            else
            {
                var orderHeaderList = _unitofWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value);
                pendingCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusPending).Count();
                inProcessCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusInProcess).Count();
                shippedCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusShipped).Count();
                rejectedCount = orderHeaderList.Where(o => o.OrderStatus == SD.OrderStatusRejected).Count();
            }


            return Json(new { pendingCount, inProcessCount, shippedCount, rejectedCount });
           
        }

        #endregion
    }
}