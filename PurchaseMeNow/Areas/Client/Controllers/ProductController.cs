using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseMeNow.DataAccess.Data.Repository.IRepository;
using PurchaseMeNow.Models;
using PurchaseMeNow.Utility;

namespace PurchaseMeNow.Areas.Client.Controllers
{
    [Area("Client")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //update order count in navbar if user is logged in 
            if(claim != null)
            {
                var count = _unitOfWork.Order
                 .GetAll(u => u.ApplicationUserId == claim.Value)
                 .ToList().Count();

                HttpContext.Session.SetInt32(SD.ssOrder, count);
            }
            return View();
        }

        public IActionResult AddToOrder(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,Department");

            Order order = new Order()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToOrder(Order orderObj)
        {
            orderObj.Id = 0;
            if (ModelState.IsValid)
            {
                //add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderObj.ApplicationUserId = claim.Value;

               Order orderFromDb = _unitOfWork.Order.GetFirstOrDefault(u => u.ApplicationUserId ==
               orderObj.ApplicationUserId && u.ProductId == orderObj.ProductId, includeProperties: "Product");

                if(orderFromDb == null)
                {
                    //add order in database for user since none exists
                    _unitOfWork.Order.Add(orderObj);
                }
                else
                {
                    orderFromDb.Count += orderObj.Count;
                    _unitOfWork.Order.Update(orderFromDb);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.Order
                    .GetAll(u => u.ApplicationUserId == orderObj.ApplicationUserId)
                    .ToList().Count();

                //add count to sessions for navlink display of order counts
                HttpContext.Session.SetInt32(SD.ssOrder, count);

                return RedirectToAction(nameof(Index));

            }
            else
            {
                //return to view
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == orderObj.ProductId, includeProperties: "Category,Department");

                Order order = new Order()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };

                return View(order);
            }

            
        }



        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties: "Category,Department");

            return Json(new { data = allObj });
        }
        #endregion
    }


}

