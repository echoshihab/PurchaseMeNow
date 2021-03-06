﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Models.ViewModels;
using PurchaseMeNow.Utility;

namespace PurchaseMeNow.Areas.Client.Controllers
{
    [Area("Client")]
    public class OrderController : Controller

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, IEmailSender emailSender,  UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;

        }

        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderListVM OrderListVM = new OrderListVM()
            {
                OrderHeader = new OrderHeader(),
                LocationList = _unitOfWork.Location.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                OrderList = _unitOfWork.Order.GetAll(u => u.ApplicationUserId == claim.Value, 
                                                    includeProperties:"Product")
            };

            OrderListVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                                                        .GetFirstOrDefault(u => u.Id == claim.Value,
                                                         includeProperties: "Department");


            return View(OrderListVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(OrderListVM orderListVM)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            orderListVM.OrderHeader = new OrderHeader(){
            OrderDate= DateTime.Now,
            OrderStatus=SD.OrderStatusPending,
            ApplicationUserId = claim.Value,
            LocationId = orderListVM.SelectedLocationId
            };
            orderListVM.OrderList = _unitOfWork.Order.GetAll(u => u.ApplicationUserId == claim.Value);
           

            _unitOfWork.OrderHeader.Add(orderListVM.OrderHeader);
            _unitOfWork.Save(); //need to save here so that orderheader ID is populated


            foreach (var item in orderListVM.OrderList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = orderListVM.OrderHeader.Id,
                    Count = item.Count

                };
                _unitOfWork.OrderDetail.Add(orderDetail);
               
            }

            _unitOfWork.Order.RemoveRange(orderListVM.OrderList);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ssOrder, 0);

            return RedirectToAction("OrderConfirmation", "Order", new { id = orderListVM.OrderHeader.Id });

        }


        public async Task<IActionResult> ResendEmail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to find user!");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Increment(int orderId)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "Product");

            order.Count += 1;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int orderId)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "Product");

            if(order.Count == 1) //remove item from shopping cart and navlink order count
            {
                var orderCount = _unitOfWork.Order.GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList().Count();
                _unitOfWork.Order.Remove(order);
                HttpContext.Session.SetInt32(SD.ssOrder, orderCount - 1);
                

            }else
            {
                order.Count -= 1;
            }
     

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete (int orderId)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "Product");

  
                var orderCount = _unitOfWork.Order.GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList().Count();
                _unitOfWork.Order.Remove(order);
                HttpContext.Session.SetInt32(SD.ssOrder, orderCount - 1);


            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

       

        public IActionResult OrderConfirmation(int id) {

            return View(id);
        }
    }
}