using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty]
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

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPOST()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder()
        {
            //start from here- null reference,need to make orderlistvm post
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            OrderListVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(u => u.Id == claim.Value, includeProperties: "Department");

            OrderListVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            OrderListVM.OrderHeader.ApplicationUserId = claim.Value;
            OrderListVM.OrderHeader.OrderDate = DateTime.Now;

            _unitOfWork.OrderHeader.Add(OrderListVM.OrderHeader);
            _unitOfWork.Save(); //need to save here so that orderheader ID is populated

            List<OrderDetail> orderDetailList = new List<OrderDetail>();

            foreach(var item in OrderListVM.OrderList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = OrderListVM.OrderHeader.Id,
                    Count = item.Count

                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            _unitOfWork.Order.RemoveRange(OrderListVM.OrderList);
            HttpContext.Session.SetInt32(SD.ssOrder, 0);

            return RedirectToAction("OrderConfirmation", "Order", new { id = OrderListVM.OrderHeader.Id });

        }

        public IActionResult OrderConfirmation(int id) {

            return View(id);
        }
    }
}