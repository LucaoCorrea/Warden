using Microsoft.AspNetCore.Mvc;
using Warden.Services;
using ClosedXML.Excel;

namespace Warden.Controllers
{
    public class CashRegisterController : Controller
    {
        private readonly CashRegisterService _cashService;

        public CashRegisterController(CashRegisterService cashService)
        {
            _cashService = cashService;
        }

        public IActionResult Index()
        {
            var openCash = _cashService.GetOpenRegister();

            if (TempData["Error"] is string errorMessage)
                ViewBag.Error = errorMessage;

            return View(openCash);
        }

        [HttpPost]
        public IActionResult Open()
        {
            try
            {
                _cashService.OpenRegister();
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Close(decimal finalAmount)
        {
            var success = _cashService.CloseRegister(finalAmount);

            if (!success)
                TempData["Error"] = "Nenhum caixa aberto para ser fechado.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult History()
        {
            var all = _cashService.GetAll();
            return View(all);
        }

        [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Withdraw(decimal amount, string reason)
        {
            var success = _cashService.Withdraw(amount, reason);
            if (!success)
                TempData["Error"] = "Não foi possível realizar a retirada.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Export(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var cashRegisters = _cashService.GetAll()
                                .Where(cr => cr.OpenedAt >= startDate && cr.OpenedAt <= endDate)
                                .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Caixas");

            worksheet.Cell("A1").Value = "ID";
            worksheet.Cell("B1").Value = "Aberto Por";
            worksheet.Cell("C1").Value = "Data Abertura";
            worksheet.Cell("D1").Value = "Valor Inicial";
            worksheet.Cell("E1").Value = "Fechado Por";
            worksheet.Cell("F1").Value = "Data Fechamento";
            worksheet.Cell("G1").Value = "Valor Final";
            worksheet.Cell("H1").Value = "Diferença";

            int currentRow = 2;
            foreach (var cr in cashRegisters)
            {
                worksheet.Cell(currentRow, 1).Value = cr.Id;
                worksheet.Cell(currentRow, 2).Value = cr.OpenedBy;
                worksheet.Cell(currentRow, 3).Value = cr.OpenedAt.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(currentRow, 4).Value = cr.InitialAmount;
                worksheet.Cell(currentRow, 5).Value = cr.ClosedBy ?? "-";
                worksheet.Cell(currentRow, 6).Value = cr.ClosedAt?.ToString("dd/MM/yyyy HH:mm") ?? "-";
                worksheet.Cell(currentRow, 7).Value = cr.FinalAmount?.ToString("F2") ?? "-";
                worksheet.Cell(currentRow, 8).Value = cr.Difference?.ToString("F2") ?? "-";
                currentRow++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"RelatorioCaixas_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

        [HttpGet]
        public IActionResult GetCurrent()
        {
            var current = _cashService.GetOpenRegister();
            if (current == null)
                return NotFound();

            return Json(new
            {
                openedAt = current.OpenedAt.ToString("dd/MM/yyyy HH:mm"),
                initialAmount = current.InitialAmount.ToString("F2")
            });
        }
    }
}
