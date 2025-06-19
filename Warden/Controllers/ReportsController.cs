using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Warden.Data;
using Warden.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Warden.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private const int LOW_STOCK_THRESHOLD = 10;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SalesReport(SalesReportFilterViewModel filters)
        {
            var query = _context.Sales.Include(s => s.Items).AsQueryable();

            if (filters.StartDate.HasValue)
                query = query.Where(s => s.SaleDate >= filters.StartDate.Value);

            if (filters.EndDate.HasValue)
                query = query.Where(s => s.SaleDate <= filters.EndDate.Value);

            if (!string.IsNullOrEmpty(filters.PaymentMethod) && Enum.TryParse(filters.PaymentMethod, out Enums.PaymentMethodEnum pm))
                query = query.Where(s => s.PaymentMethod == pm);

            if (!string.IsNullOrEmpty(filters.UserName))
                query = query.Where(s => s.UserName == filters.UserName);

            var sales = query.Select(s => new SalesReportItem
            {
                SaleId = s.Id,
                SaleDate = s.SaleDate,
                UserName = s.UserName,
                PaymentMethod = s.PaymentMethod.ToString(),
                TotalAmount = s.Items.Sum(i => i.Quantity * i.UnitPrice)
            })
            .OrderByDescending(s => s.SaleDate)
            .ToList();

            var vm = new SalesReportViewModel
            {
                Filters = filters,
                Sales = sales
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult StockReport(StockReportFilterViewModel filters)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Category))
                query = query.Where(p => p.Category == filters.Category);

            var products = query.Select(p => new StockReportItem
            {
                ProductName = p.Name,
                Category = p.Category,
                Stock = p.Stock,
                IsLowStock = p.Stock <= LOW_STOCK_THRESHOLD
            })
            .OrderBy(p => p.ProductName)
            .ToList();

            var vm = new StockReportViewModel
            {
                Filters = filters,
                Products = products
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult ProfitReport(ProfitReportFilterViewModel filters)
        {
            var query = _context.Sales.Include(s => s.Items).AsQueryable();

            if (filters.StartDate.HasValue)
                query = query.Where(s => s.SaleDate >= filters.StartDate.Value);

            if (filters.EndDate.HasValue)
                query = query.Where(s => s.SaleDate <= filters.EndDate.Value);

            var sales = query.ToList();

            var totalSales = sales.Sum(s => s.Items.Sum(i => i.Quantity * i.UnitPrice));
            var totalCost = sales.Sum(s => s.Items.Sum(i =>
                _context.Products.FirstOrDefault(p => p.Id == i.ProductId).CostPrice * i.Quantity
            ));

            var vm = new ProfitReportViewModel
            {
                Filters = filters,
                TotalSales = totalSales,
                TotalCost = totalCost,
                TotalProfit = totalSales - totalCost
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult UserSalesReport()
        {
            var userSales = _context.Sales
                .GroupBy(s => s.UserName)
                .Select(g => new UserSalesReportItem
                {
                    UserName = g.Key,
                    TotalSalesAmount = g.SelectMany(s => s.Items).Sum(i => i.Quantity * i.UnitPrice)
                })
                .OrderByDescending(u => u.TotalSalesAmount)
                .ToList();

            var vm = new UserSalesReportViewModel
            {
                UserSales = userSales
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult ExportStockToExcel(StockReportFilterViewModel filters)
        {
            var data = GetStockData(filters);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Estoque");

            worksheet.Cell(1, 1).Value = "Produto";
            worksheet.Cell(1, 2).Value = "Categoria";
            worksheet.Cell(1, 3).Value = "Estoque";

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = data[i].ProductName;
                worksheet.Cell(i + 2, 2).Value = data[i].Category;
                worksheet.Cell(i + 2, 3).Value = data[i].Stock;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Relatorio_Estoque.xlsx");
        }

        [HttpGet]
        public IActionResult ExportStockToPDF(StockReportFilterViewModel filters)
        {
            var data = GetStockData(filters);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
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
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.ProductName);
                            table.Cell().Element(CellStyle).Text(item.Category);
                            table.Cell().Element(CellStyle).Text(item.Stock.ToString());
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.PaddingVertical(5).PaddingHorizontal(10);
                        }
                    });
                });
            });

            var pdf = document.GeneratePdf();
            return File(pdf, "application/pdf", "Relatorio_Estoque.pdf");
        }

        private List<StockReportItem> GetStockData(StockReportFilterViewModel filters)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Category))
                query = query.Where(p => p.Category == filters.Category);

            return query.Select(p => new StockReportItem
            {
                ProductName = p.Name,
                Category = p.Category,
                Stock = p.Stock,
                IsLowStock = p.Stock <= LOW_STOCK_THRESHOLD
            }).ToList();
        }
    }
}
