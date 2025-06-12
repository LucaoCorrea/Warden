using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warden.Models;
using Warden.Services;
using Warden.Repositories;

namespace Warden.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleService _saleService;
        private readonly IProductRepository _productRepo;

        public SaleController(SaleService saleService, IProductRepository productRepo)
        {
            _saleService = saleService;
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            var sales = _saleService.GetAll(); 
            return View(sales);
        }

        public IActionResult Create()
        {
            var products = _productRepo.GetAll();
            ViewBag.Products = products; 
            return View();
        }

        [HttpPost]
        public IActionResult Create(SaleModel sale)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _productRepo.GetAll(); 
                return View(sale);
            }

            _saleService.ProcessSale(sale);
            return RedirectToAction("Index");
        }
    }
}
