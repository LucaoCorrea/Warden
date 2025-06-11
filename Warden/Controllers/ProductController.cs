using Microsoft.AspNetCore.Mvc;
using Warden.Models;
using Warden.Services;

namespace Warden.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service; 

        public ProductController(ProductService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var products = _service.GetAll(); 
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(ProductModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            _service.Add(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel product)
        {
            if (!ModelState.IsValid)
                return View(product);
            _service.Update(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
    }
}
