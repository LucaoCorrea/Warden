using Microsoft.AspNetCore.Mvc;
using Warden.Models;
using Warden.Services;
using ClosedXML.Excel;

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

        public IActionResult Delete(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var allProducts = _service.GetAll();

            // verificacoes simplificadas
            var nameExists = allProducts.Any(p => p.Name.ToLower() == product.Name.ToLower());
            if (nameExists)
            {
                ModelState.AddModelError("Name", "Já existe um produto com este nome.");
                return View(product);
            }

            var skuExists = allProducts.Any(p => p.SKU.ToLower() == product.SKU.ToLower());
            if (skuExists)
            {
                ModelState.AddModelError("SKU", "Já existe um produto com este SKU.");
                return View(product);
            }

            _service.Add(product);
            return RedirectToAction("Index");
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
            if (!ModelState.IsValid) {
                return View(product);
            }
               
            _service.Update(product);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ExportToExcel()
        {
            var products = _service.GetAll();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Produtos");

            worksheet.Cell("A1").Value = "Nome";
            worksheet.Cell("B1").Value = "SKU";
            worksheet.Cell("C1").Value = "Categoria";
            worksheet.Cell("D1").Value = "Estoque";
            worksheet.Cell("E1").Value = "Preço de Venda";

            int row = 2;
            foreach (var product in products)
            {
                worksheet.Cell(row, 1).Value = product.Name;
                worksheet.Cell(row, 2).Value = product.SKU;
                worksheet.Cell(row, 3).Value = product.Category;
                worksheet.Cell(row, 4).Value = product.Stock;
                worksheet.Cell(row, 5).Value = product.SalePrice;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Produtos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

    }
}
