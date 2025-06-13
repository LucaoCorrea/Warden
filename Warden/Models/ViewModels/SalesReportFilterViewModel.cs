namespace Warden.Models.ViewModels
{
    public class SalesReportFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? UserName { get; set; }
    }

    public class SalesReportViewModel
    {
        public List<SalesReportItem> Sales { get; set; }
        public SalesReportFilterViewModel Filters { get; set; }
    }

    public class SalesReportItem
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string UserName { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class StockReportFilterViewModel
    {
        public string? Category { get; set; }
    }

    public class StockReportViewModel
    {
        public List<StockReportItem> Products { get; set; }
        public StockReportFilterViewModel Filters { get; set; }
    }

    public class StockReportItem
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public bool IsLowStock { get; set; }
    }

    public class ProfitReportFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ProfitReportViewModel
    {
        public decimal TotalCost { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalProfit { get; set; }
        public ProfitReportFilterViewModel Filters { get; set; }
    }

    public class UserSalesReportViewModel
    {
        public List<UserSalesReportItem> UserSales { get; set; }
    }

    public class UserSalesReportItem
    {
        public string UserName { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
