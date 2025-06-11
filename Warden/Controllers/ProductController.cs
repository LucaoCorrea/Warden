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
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            _service.Add(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            _service.Update(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
