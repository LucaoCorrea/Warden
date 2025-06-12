using Microsoft.AspNetCore.Mvc;
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
            if (!ModelState.IsValid || sale.Items == null || !sale.Items.Any())
            {
                ViewBag.Products = _productRepo.GetAll();
                ModelState.AddModelError("", "Por favor, adicione pelo menos um item à venda.");
                return View(sale);
            }

            try
            {
                var saleId = _saleService.ProcessSale(sale);
                return RedirectToAction(nameof(Receipt), new { id = saleId });
            }
            catch (Exception ex)
            {
                ViewBag.Products = _productRepo.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(sale);
            }
        }

        public IActionResult Receipt(int id)
        {
            var sale = _saleService.GetById(id);
            if (sale == null) return NotFound();
            return View(sale);
        }

        public IActionResult DownloadReceipt(int id)
        {
            var sale = _saleService.GetById(id);
            if (sale == null) return NotFound();

            var receiptText = GenerateFakeReceiptText(sale);

            var bytes = System.Text.Encoding.UTF8.GetBytes(receiptText);
            return File(bytes, "text/plain", $"NotaFiscal_Venda_{id}.txt");
        }

        private string GenerateFakeReceiptText(SaleModel sale)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("NOTA FISCAL (FALSA)");
            sb.AppendLine($"Venda ID: {sale.Id}");
            sb.AppendLine($"Usuário: {sale.UserName}");
            sb.AppendLine($"Forma de Pagamento: {sale.PaymentMethod}");
            sb.AppendLine("Itens:");
            decimal total = 0;
            foreach (var item in sale.Items)
            {
                sb.AppendLine($"- Produto ID: {item.ProductId} | Qtde: {item.Quantity} | Unitário: R$ {item.UnitPrice:F2} | Total: R$ {(item.Quantity * item.UnitPrice):F2}");
                total += item.Quantity * item.UnitPrice;
            }
            sb.AppendLine($"TOTAL DA VENDA: R$ {total:F2}");
            sb.AppendLine("Obrigado pela compra!");
            return sb.ToString();
        }
    }
}
