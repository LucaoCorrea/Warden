using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.ProductId = new SelectList(_productService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(StockMovementModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = new SelectList(_productService.GetAll(), "Id", "Name");
                return View(model);
            }

            _service.Add(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
