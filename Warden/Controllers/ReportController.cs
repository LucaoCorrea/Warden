using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using Warden.Data;
using Warden.Models.ViewModels;

namespace Warden.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private const int LOW_STOCK_THRESHOLD = 10;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate, string paymentMethod, string userName)
        {
            var query = _context.Sales.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.SaleDate >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(s => s.SaleDate <= endDate.Value.Date.AddDays(1).AddTicks(-1));

            if (!string.IsNullOrEmpty(paymentMethod))
                query = query.Where(s => s.PaymentMethod.ToString() == paymentMethod);

            if (!string.IsNullOrEmpty(userName))
                query = query.Where(s => s.UserName == userName);

            var sales = await query
                .Include(s => s.Items)
                .Select(s => new SalesReportItem
                {
                    SaleId = s.Id,
                    SaleDate = s.SaleDate,
                    UserName = s.UserName,
                    PaymentMethod = s.PaymentMethod.ToString(),
                    TotalAmount = s.Items.Sum(i => i.Quantity * i.UnitPrice)
                })
                .ToListAsync();

            var viewModel = new SalesReportViewModel
            {
                Sales = sales,
                Filters = new SalesReportFilterViewModel
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentMethod = paymentMethod,
                    UserName = userName
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel(DateTime? startDate, DateTime? endDate, string paymentMethod, string userName)
        {
            var data = await GetSalesData(startDate, endDate, paymentMethod, userName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Vendas");

            worksheet.Cell(1, 1).Value = "ID Venda";
            worksheet.Cell(1, 2).Value = "Data";
            worksheet.Cell(1, 3).Value = "Usuário";
            worksheet.Cell(1, 4).Value = "Forma de Pagamento";
            worksheet.Cell(1, 5).Value = "Total (R$)";

            int row = 2;
            foreach (var sale in data)
            {
                worksheet.Cell(row, 1).Value = sale.SaleId;
                worksheet.Cell(row, 2).Value = sale.SaleDate.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(row, 3).Value = sale.UserName;
                worksheet.Cell(row, 4).Value = sale.PaymentMethod;
                worksheet.Cell(row, 5).Value = sale.TotalAmount;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "RelatorioVendas.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(DateTime? startDate, DateTime? endDate, string paymentMethod, string userName)
        {
            var data = await GetSalesData(startDate, endDate, paymentMethod, userName);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header().Text("Relatório de Vendas").SemiBold().FontSize(20).FontColor(Colors.Red.Medium);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID Venda");
                            header.Cell().Element(CellStyle).Text("Data");
                            header.Cell().Element(CellStyle).Text("Usuário");
                            header.Cell().Element(CellStyle).Text("Pagamento");
                            header.Cell().Element(CellStyle).Text("Total (R$)");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten2);
                            }
                        });

                        foreach (var sale in data)
                        {
                            table.Cell().Element(Cell).Text(sale.SaleId);
                            table.Cell().Element(Cell).Text(sale.SaleDate.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Element(Cell).Text(sale.UserName);
                            table.Cell().Element(Cell).Text(sale.PaymentMethod);
                            table.Cell().Element(Cell).Text(sale.TotalAmount.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")));

                            static IContainer Cell(IContainer container) => container.Padding(5);
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Warden - ").SemiBold();
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy"));
                    });
                });
            });

            var stream = new MemoryStream();
            pdf.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "RelatorioVendas.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ExportStockToExcel(string category)
        {
            var data = await GetStockData(category);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Estoque");

            worksheet.Cell(1, 1).Value = "Produto";
            worksheet.Cell(1, 2).Value = "Categoria";
            worksheet.Cell(1, 3).Value = "Estoque";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.ProductName;
                worksheet.Cell(row, 2).Value = item.Category;
                worksheet.Cell(row, 3).Value = item.Stock;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Relatorio_Estoque.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportStockToPdf(string category)
        {
            var data = await GetStockData(category);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header().Text("Relatório de Estoque").SemiBold().FontSize(20).FontColor(Colors.Red.Medium);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Produto");
                            header.Cell().Element(CellStyle).Text("Categoria");
                            header.Cell().Element(CellStyle).Text("Estoque");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten2);
                            }
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(Cell).Text(item.ProductName);
                            table.Cell().Element(Cell).Text(item.Category);
                            table.Cell().Element(Cell).Text(item.Stock.ToString());

                            static IContainer Cell(IContainer container) => container.Padding(5);
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Warden - ").SemiBold();
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy"));
                    });
                });
            });

            var stream = new MemoryStream();
            pdf.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "Relatorio_Estoque.pdf");
        }

        private async Task<List<StockReportItem>> GetStockData(string category)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            return await query.Select(p => new StockReportItem
            {
                ProductName = p.Name,
                Category = p.Category,
                Stock = p.Stock
            }).ToListAsync();
        }

        private async Task<List<SalesReportItem>> GetSalesData(DateTime? startDate, DateTime? endDate, string paymentMethod, string userName)
        {
            var query = _context.Sales.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.SaleDate >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(s => s.SaleDate <= endDate.Value.Date.AddDays(1).AddTicks(-1));

            if (!string.IsNullOrEmpty(paymentMethod))
                query = query.Where(s => s.PaymentMethod.ToString() == paymentMethod);

            if (!string.IsNullOrEmpty(userName))
                query = query.Where(s => s.UserName == userName);

            return await query
                .Include(s => s.Items)
                .Select(s => new SalesReportItem
                {
                    SaleId = s.Id,
                    SaleDate = s.SaleDate,
                    UserName = s.UserName,
                    PaymentMethod = s.PaymentMethod.ToString(),
                    TotalAmount = s.Items.Sum(i => i.Quantity * i.UnitPrice)
                })
                .ToListAsync();
        }
    }
}
