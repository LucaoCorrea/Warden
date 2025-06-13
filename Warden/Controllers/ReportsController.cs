using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models.ViewModels;

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

            if (!string.IsNullOrEmpty(filters.PaymentMethod))
            {
                if (Enum.TryParse(filters.PaymentMethod, out Enums.PaymentMethodEnum pm))
                    query = query.Where(s => s.PaymentMethod == pm);
            }

            if (!string.IsNullOrEmpty(filters.UserName))
                query = query.Where(s => s.UserName == filters.UserName);

            var sales = query
                .Select(s => new SalesReportItem
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

            var products = query
                .Select(p => new StockReportItem
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

            decimal totalSales = 0m;
            decimal totalCost = 0m;

            foreach (var sale in sales)
            {
                foreach (var item in sale.Items)
                {
                    totalSales += item.Quantity * item.UnitPrice;
                    var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                        totalCost += item.Quantity * product.CostPrice;
                }
            }

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
    }
}
