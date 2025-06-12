using Microsoft.AspNetCore.Mvc;
using Warden.Services;

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
    }
}
