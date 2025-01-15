using BABERSHOP.DataAccess.Repository.IRepository;
using BABERSHOP.Models;
using BABERSHOP.Models.ViewModels;
using BABERSHOPWEB.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace BABERSHOPWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnityOfWork unityOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unityOfWork = unityOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ObjProducts = _unityOfWork.Product.GetAll(includeProperties: "ProductType").ToList();
            return View(ObjProducts);
        }

        public IActionResult Upsert(int? ProductId)
        {
            ProductVM productVM = new() 
            {
                ProductCategoryList = _unityOfWork.PCategory.GetAll().Select(u => new SelectListItem
                {
                    Text = u.ProductType_Name,
                    Value = u.ProductTypeId.ToString()
                }),
                Product = new Product()
            };
            if (ProductId == null || ProductId == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unityOfWork.Product.GeT(u => u.ProductId == ProductId);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImangeUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImangeUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream); 
                    }

                    productVM.Product.ImangeUrl = @"images\product" + fileName;
                }
                if(productVM.Product.ProductId != 0)
                {
                    _unityOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unityOfWork.Product.Update(productVM.Product);
                }

                _unityOfWork.Save();
                TempData["success"] = "Catergory created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.ProductCategoryList = _unityOfWork.PCategory.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.ProductType_Name,
                        Value = u.ProductTypeId.ToString()
                    });
                return View(productVM);
            }
            
        }

        //This Delete endpoint should be Removed since we using Delete API Call now
        public IActionResult Delete(int? ProductId)
        {
            if (ProductId == null || ProductId == 0)
            {
                return NotFound();
            }

            Product? productFromDB = _unityOfWork.Product.GeT(u => u.ProductId == ProductId);

            if (productFromDB == null)
            {
                return NotFound();
            }

            return View(productFromDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? ProductId)
        {
            Product? obj = _unityOfWork.Product.GeT(u => u.ProductId == ProductId);

            if (obj == null)
            {
                return NotFound();
            }

            _unityOfWork.Product.Remove(obj);
            _unityOfWork.Save();
            TempData["success"] = "Catergory deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        //TABLE API CALL

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ObjProducts = _unityOfWork.Product.GetAll(includeProperties: "ProductType").ToList();
            return Json(new { data = ObjProducts });

        }      
        
        
        //[HttpDelete]
        //public IActionResult Delete(int ProductId)
        //{
        //    var productToBeDeleted = _unityOfWork.Product.GeT(u => u.ProductId == ProductId);
        //    if (productToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting!"});
        //    }

        //    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImangeUrl.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unityOfWork.Product.Remove(productToBeDeleted);
        //    _unityOfWork.Save();

        //    return Json(new { success = true, message = "Deleted Successful" });
        //}
        #endregion

    }
}
