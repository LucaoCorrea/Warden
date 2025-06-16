using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models.ViewModels;

namespace Warden.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

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
                .Select(s => new SalesReportItem
                {
                    SaleId = s.Id,
                    SaleDate = s.SaleDate,
                    UserName = s.UserName,
                    PaymentMethod = s.PaymentMethod.ToString()
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
    }
}
