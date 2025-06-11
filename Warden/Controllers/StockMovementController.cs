using Microsoft.AspNetCore.Mvc;
using Warden.Models;
using Warden.Services;

namespace Warden.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly StockMovementService _service; 
        private readonly ProductService _productService; 

        public StockMovementController(StockMovementService service, ProductService productService)
        {
            _service = service;
            _productService = productService;
        }

        public IActionResult Index() => View(_service.GetAll());

        public IActionResult Create()
        {
            ViewBag.Products = _productService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(StockMovementModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _productService.GetAll();
                return View(model);
            }

            _service.Add(model);
            return RedirectToAction(nameof(Index));
        }

    }
}
