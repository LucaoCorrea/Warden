using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Warden.Models;
using Warden.Repositories;
using Warden.Repository;
using Warden.Services;

namespace Warden.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleService _saleService;
        private readonly IProductRepository _productRepo;
        private readonly CashRegisterService _cashService;
        private readonly ICustomerRepository _customerRepo;

        public SaleController(SaleService saleService, IProductRepository productRepo, CashRegisterService cashService, ICustomerRepository customerRepo)
        {
            _saleService = saleService;
            _productRepo = productRepo;
            _cashService = cashService;
            _customerRepo = customerRepo;
        }

        public IActionResult Index()
        {
            var sales = _saleService.GetAll().ToList();

            foreach (var sale in sales)
            {
                if (sale.LoyalCustomerId.HasValue)
                {
                    sale.LoyalCustomer = _customerRepo.GetById(sale.LoyalCustomerId.Value);
                }
            }

            return View(sales);
        }

        public IActionResult Create()
        {
            var openCash = _cashService.GetOpenRegister();
            if (openCash == null)
            {
                TempData["Error"] = "Você precisa abrir o caixa antes de iniciar uma venda.";
                return RedirectToAction("Index", "CashRegister");
            }

            var products = _productRepo.GetAll();
            ViewBag.Products = products;
            ViewBag.CustomerList = new SelectList(_customerRepo.GetAll(), "Id", "Name");

            return View();
        }

        // metodo para criação de vendas PDV

        [HttpPost]
        public IActionResult Create(SaleModel sale, bool ApplyCashback, decimal CashbackUsed)
        {
            var openCash = _cashService.GetOpenRegister();
            if (openCash == null)
            {
                TempData["Error"] = "Você precisa abrir o caixa antes de concluir uma venda.";
                return RedirectToAction("Index", "CashRegister");
            }

            if (!ModelState.IsValid || sale.Items == null || !sale.Items.Any())
            {
                ViewBag.Products = _productRepo.GetAll();
                ViewBag.CustomerList = new SelectList(_customerRepo.GetAll(), "Id", "Name");
                ModelState.AddModelError("", "Por favor, adicione pelo menos um item à venda.");
                return View(sale);
            }

            LoyalCustomerModel? customer = null;

            if (sale.LoyalCustomerId.HasValue)
            {
                customer = _customerRepo.GetById(sale.LoyalCustomerId.Value);
                if (customer == null)
                {
                    ViewBag.Products = _productRepo.GetAll();
                    ViewBag.CustomerList = new SelectList(_customerRepo.GetAll(), "Id", "Name");
                    ModelState.AddModelError("LoyalCustomerId", "Cliente fidelizado não encontrado.");
                    return View(sale);
                }

                sale.LoyalCustomer = customer;
            }

            try
            {
            
                if (ApplyCashback && customer != null && CashbackUsed > 0)
                {
                    CashbackUsed = Math.Min(CashbackUsed, customer.CashbackBalance);
                    CashbackUsed = Math.Min(CashbackUsed, sale.TotalAmount);

                    sale.CashbackUsed = CashbackUsed;
                    sale.TotalAmount -= CashbackUsed;  

                    customer.CashbackBalance -= CashbackUsed;
                    _customerRepo.Update(customer);
                }
                else
                {
                    sale.CashbackUsed = 0;
                }

                var saleId = _saleService.ProcessSale(sale);

                return RedirectToAction(nameof(Receipt), new { id = saleId });
            }
            catch (Exception ex)
            {
                ViewBag.Products = _productRepo.GetAll();
                ViewBag.CustomerList = new SelectList(_customerRepo.GetAll(), "Id", "Name");
                ModelState.AddModelError("", ex.Message);
                return View(sale);
            }
        }



        [HttpGet]
        public IActionResult GetCashback(int customerId)
        {
            var customer = _customerRepo.GetById(customerId);
            if (customer == null)
                return Json(new { cashback = 0m });

            return Json(new { cashback = customer.CashbackBalance });
        }

        public IActionResult Receipt(int id)
        {
            var sale = _saleService.GetById(id);
            if (sale == null) return NotFound();

            if (sale.LoyalCustomer == null && sale.LoyalCustomerId.HasValue)
            {
                sale.LoyalCustomer = _customerRepo.GetById(sale.LoyalCustomerId.Value);
            }

            return View(sale);
        }

        public IActionResult DownloadReceipt(int id)
        {
            var sale = _saleService.GetById(id);
            if (sale == null) return NotFound();

            var pdfBytes = GeneratePdfReceipt(sale);

            return File(pdfBytes, "application/pdf", $"NotaFiscal_Venda_{id}.pdf");
        }

        private byte[] GeneratePdfReceipt(SaleModel sale)
        {
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logo.png");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                    page.Header()
                        .Height(80)
                        .Row(row =>
                        {
                            row.RelativeColumn()
                                .AlignMiddle()
                                .Image(logoPath, ImageScaling.FitHeight);

                            row.RelativeColumn()
                                .AlignMiddle()
                                .Text("Warden - Sistema de Estoque")
                                .FontColor(Colors.Red.Darken1);

                            row.ConstantColumn(250)
                                .AlignMiddle()
                                .Text("NOTA FISCAL (FALSA)")
                                .SemiBold()
                                .FontSize(20)
                                .FontColor(Colors.Green.Darken2)
                                .AlignRight();
                        });

                    page.Content()
                        .PaddingVertical(10)
                        .Column(col =>
                        {
                            col.Spacing(5);

                            col.Item().Text($"Venda ID: {sale.Id}").SemiBold();
                            col.Item().Text($"Vendedor: {sale.UserName}");
                            col.Item().Text($"Forma de Pagamento: {sale.PaymentMethod}");

                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().Text("Itens da Venda:").Bold().FontSize(14);

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(250);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Produto").Bold();
                                    header.Cell().AlignRight().Text("Qtd").Bold();
                                    header.Cell().AlignRight().Text("Unitário").Bold();
                                    header.Cell().AlignRight().Text("Total").Bold();
                                });

                                decimal totalSale = 0;

                                foreach (var item in sale.Items)
                                {
                                    decimal totalItem = item.Quantity * item.UnitPrice;
                                    totalSale += totalItem;

                                    table.Cell().Text($"Produto #{item.ProductId}");
                                    table.Cell().AlignRight().Text(item.Quantity.ToString());
                                    table.Cell().AlignRight().Text($"R$ {item.UnitPrice:F2}");
                                    table.Cell().AlignRight().Text($"R$ {totalItem:F2}");
                                }

                                table.Footer(footer =>
                                {
                                    footer.Cell().ColumnSpan(3).AlignRight().Text("TOTAL DA VENDA:").Bold();
                                    footer.Cell().AlignRight().Text($"R$ {totalSale:F2}").Bold();
                                });
                            });

                            col.Item().PaddingTop(20).Text("Obrigado pela compra e pela preferência de usar WARDEN!").Italic().FontSize(14).FontColor(Colors.Grey.Darken1);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Warden © " + DateTime.Now.Year);
                        });
                });
            });

            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }

        [HttpPost]
        public IActionResult Export(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var sales = _saleService.GetAll()
                        .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                        .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Vendas");

            worksheet.Cell("A1").Value = "ID Venda";
            worksheet.Cell("B1").Value = "Data";
            worksheet.Cell("C1").Value = "Vendedor";
            worksheet.Cell("D1").Value = "Forma de Pagamento";
            worksheet.Cell("E1").Value = "Quantidade Itens";

            int currentRow = 2;

            foreach (var sale in sales)
            {
                worksheet.Cell(currentRow, 1).Value = sale.Id;
                worksheet.Cell(currentRow, 2).Value = sale.SaleDate.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(currentRow, 3).Value = sale.UserName;
                worksheet.Cell(currentRow, 4).Value = sale.PaymentMethod.ToString();
                worksheet.Cell(currentRow, 5).Value = sale.Items?.Count ?? 0;
                currentRow++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorio_vendas.xlsx");
        }
    }
}
