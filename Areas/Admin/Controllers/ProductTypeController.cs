using BABERSHOP.DataAccess.Repository.IRepository;
using BABERSHOP.Models;
using BABERSHOPWEB.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BABERSHOPWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;

        public ProductTypeController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }
        public IActionResult Index()
        {
            List<ProductType> ObjProductTypes = _unityOfWork.PCategory.GetAll().ToList();
            return View(ObjProductTypes);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductType obj)
        {
            if (obj.ProductType_Name.ToString() == obj.ProductType_Description.ToString())
            {
                ModelState.AddModelError("producttype_name", "Product category name can not be the same as it's Description!");
            }
            if (ModelState.IsValid)
            {
                _unityOfWork.PCategory.Add(obj);
                _unityOfWork.Save();
                TempData["success"] = "Catergory created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? ProductTypeId)
        {
            if (ProductTypeId == null || ProductTypeId == 0)
            {
                return NotFound();
            }

            ProductType? productTypeFromDB = _unityOfWork.PCategory.GeT(u => u.ProductTypeId == ProductTypeId);

            if (productTypeFromDB == null)
            {
                return NotFound();
            }

            return View(productTypeFromDB);
        }
        [HttpPost]
        public IActionResult Edit(ProductType obj)
        {
            if (obj.ProductType_Name.ToString() == obj.ProductType_Description.ToString())
            {
                ModelState.AddModelError("producttype_name", "Product category name can not be the same as it's Description!");
            }
            if (ModelState.IsValid)
            {
                _unityOfWork.PCategory.Update(obj);
                _unityOfWork.Save();
                TempData["success"] = "Catergory updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? ProductTypeId)
        {
            if (ProductTypeId == null || ProductTypeId == 0)
            {
                return NotFound();
            }

            ProductType? productTypeFromDB = _unityOfWork.PCategory.GeT(u => u.ProductTypeId == ProductTypeId);

            if (productTypeFromDB == null)
            {
                return NotFound();
            }

            return View(productTypeFromDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? ProductTypeId)
        {
            ProductType? obj = _unityOfWork.PCategory.GeT(u => u.ProductTypeId == ProductTypeId);

            if (obj == null)
            {
                return NotFound();
            }

            _unityOfWork.PCategory.Remove(obj);
            _unityOfWork.Save();
            TempData["success"] = "Catergory deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
