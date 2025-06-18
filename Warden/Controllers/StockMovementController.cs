using ClosedXML.Excel;
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

        [HttpPost]
        public IActionResult ExportToExcel()
        {
            var movements = _service.GetAll();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Movimentações");

            worksheet.Cell("A1").Value = "Produto";
            worksheet.Cell("B1").Value = "Tipo";
            worksheet.Cell("C1").Value = "Quantidade";
            worksheet.Cell("D1").Value = "Valor Total";
            worksheet.Cell("E1").Value = "Data";

            int currentRow = 2;

            foreach (var item in movements)
            {
                worksheet.Cell(currentRow, 1).Value = item.Product?.Name ?? "N/A";
                worksheet.Cell(currentRow, 2).Value = item.Type.ToString();
                worksheet.Cell(currentRow, 3).Value = item.Quantity;
                worksheet.Cell(currentRow, 4).Value = item.Sale?.TotalAmount ?? 0;  
                worksheet.Cell(currentRow, 5).Value = item.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                currentRow++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"MovimentacoesEstoque_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }
}
