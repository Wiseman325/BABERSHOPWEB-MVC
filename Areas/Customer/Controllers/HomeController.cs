using BABERSHOP.DataAccess.Repository.IRepository;
using BABERSHOP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BABERSHOPWEB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnityOfWork _unityOfWork;

        public HomeController(ILogger<HomeController> logger, IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unityOfWork.Product.GetAll(includeProperties: "ProductType");
            return View(productList);
        }    
        public IActionResult Details(int id)
        {
            Product product = _unityOfWork.Product.GeT(u => u.ProductId == id , includeProperties: "ProductType");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
