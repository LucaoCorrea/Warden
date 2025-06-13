using System;
using System.Collections.Generic;

namespace Warden.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<TopProductDto> TopProducts { get; set; }
        public List<SalesByHourDto> SalesByHour { get; set; }
        public List<PaymentMethodSalesDto> SalesByPaymentMethod { get; set; }

        public decimal TotalSalesAmount { get; set; }
        public int TotalSalesCount { get; set; }

        public decimal AverageTicket => TotalSalesCount == 0 ? 0 : TotalSalesAmount / TotalSalesCount;

        public List<SalesByCategoryDto> SalesByCategory { get; set; }
        public List<TopUserSalesDto> TopUsers { get; set; }
        public List<SalesByWeekdayDto> SalesByWeekday { get; set; }
        public List<LowStockProductDto> LowStockProducts { get; set; }
        public List<AvgTicketByPaymentMethodDto> AvgTicketByPaymentMethod { get; set; }
    }

    public class TopProductDto
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }

    public class SalesByHourDto
    {
        public int Hour { get; set; }
        public int TotalSales { get; set; }
    }

    public class PaymentMethodSalesDto
    {
        public string PaymentMethod { get; set; }
        public int SalesCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class SalesByCategoryDto
    {
        public string Category { get; set; }
        public int QuantitySold { get; set; }
    }

    public class TopUserSalesDto
    {
        public string UserName { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }

    public class SalesByWeekdayDto
    {
        public string Weekday { get; set; }
        public decimal AverageSales { get; set; }
    }

    public class LowStockProductDto
    {
        public string ProductName { get; set; }
        public int Stock { get; set; }
    }

    public class AvgTicketByPaymentMethodDto
    {
        public string PaymentMethod { get; set; }
        public decimal AverageTicket { get; set; }
    }
}
