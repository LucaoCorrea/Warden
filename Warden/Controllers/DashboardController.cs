using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models.ViewModels;

namespace Warden.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var topProducts = _context.SaleItems
                .Include(i => i.Product)
                .GroupBy(i => i.Product.Name)
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(i => i.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(5)
                .ToList();

            var salesByHour = _context.Sales
                .GroupBy(s => s.SaleDate.Hour)
                .Select(g => new SalesByHourDto
                {
                    Hour = g.Key,
                    TotalSales = g.Count()
                })
                .OrderByDescending(h => h.TotalSales)
                .ToList();

            var sales = _context.Sales
                .Include(s => s.Items)
                .AsEnumerable();

            var salesByWeekday = sales
                .GroupBy(s => s.SaleDate.DayOfWeek)
                .Select(g => new SalesByWeekdayDto
                {
                    Weekday = g.Key.ToString(),
                    AverageSales = g.Average(s => s.Items.Sum(i => i.Quantity * i.UnitPrice))
                })
                .OrderBy(d => d.Weekday)
                .ToList();

            var salesByCategory = _context.SaleItems
                .Include(i => i.Product)
                .GroupBy(i => i.Product.Category)
                .Select(g => new SalesByCategoryDto
                {
                    Category = g.Key,
                    QuantitySold = g.Sum(i => i.Quantity)
                })
                .OrderByDescending(c => c.QuantitySold)
                .ToList();

            var salesByPaymentMethod = _context.Sales
                .GroupBy(s => s.PaymentMethod.ToString())
                .Select(g => new PaymentMethodSalesDto
                {
                PaymentMethod = g.Key,
                SalesCount = g.Count()
                 })
                .OrderByDescending(p => p.SalesCount)
                .ToList();

            var totalSalesAmount = _context.SaleItems.Sum(i => i.Quantity * i.UnitPrice);
            var totalSalesCount = _context.Sales.Count();

            var topUsers = sales
                .GroupBy(s => s.UserName)
                .Select(g => new TopUserSalesDto
                {
                    UserName = g.Key,
                    TotalSalesAmount = g.Sum(s => s.Items.Sum(i => i.Quantity * i.UnitPrice))
                })
                .OrderByDescending(u => u.TotalSalesAmount)
                .Take(5)
                .ToList();

            var lowStockProducts = _context.Products
                .Where(p => p.Stock <= 10)
                .Select(p => new LowStockProductDto
                {
                    ProductName = p.Name,
                    Stock = p.Stock
                })
                .ToList();

            var avgTicketByPaymentMethod = sales
                .GroupBy(s => s.PaymentMethod.ToString())
                .Select(g => new AvgTicketByPaymentMethodDto
                {
                    PaymentMethod = g.Key,
                    AverageTicket = g.Average(s => s.Items.Sum(i => i.Quantity * i.UnitPrice))
                })
                .ToList();

            var viewModel = new DashboardViewModel
            {
                TopProducts = topProducts,
                SalesByHour = salesByHour,
                SalesByWeekday = salesByWeekday,
                SalesByCategory = salesByCategory,
                SalesByPaymentMethod = salesByPaymentMethod,
                TotalSalesAmount = totalSalesAmount,
                TotalSalesCount = totalSalesCount,
                TopUsers = topUsers,
                LowStockProducts = lowStockProducts,
                AvgTicketByPaymentMethod = avgTicketByPaymentMethod
            };

            return View(viewModel);
        }
    }
}
