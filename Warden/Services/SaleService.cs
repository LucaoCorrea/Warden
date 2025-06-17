using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Enums;
using Warden.Models;
using Warden.Repositories;
using Warden.Repository;

namespace Warden.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStockMovementRepository _stockRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly AppDbContext _context;

        public SaleService(ISaleRepository saleRepo, IProductRepository productRepo, IStockMovementRepository stockRepo, AppDbContext context, ICustomerRepository customerRepo)
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _context = context;
            _customerRepo = customerRepo;
        }

        public IEnumerable<SaleModel> GetAll()
        {
            return _context.Sales
                .Include(s => s.Items)
                .Include(s => s.LoyalCustomer) 
                .ToList();
        }

        public SaleModel GetById(int id)
        {
            return _context.Sales
                .Include(s => s.Items)
                .Include(s => s.LoyalCustomer) 
                .FirstOrDefault(s => s.Id == id);
        }

        public List<LoyalCustomerModel> GetAllLoyalCustomers()
        {
            return _context.LoyalCustomers
                           .OrderBy(c => c.Name)
                           .ToList();
        }

        public int ProcessSale(SaleModel sale)
        {
            sale.SaleDate = DateTime.Now;

            foreach (var item in sale.Items)
            {
                var product = _productRepo.GetById(item.ProductId);
                if (product == null)
                    throw new Exception($"Produto {item.ProductId} não encontrado.");

                if (product.Stock < item.Quantity)
                    throw new Exception($"Produto {product.Name} sem estoque suficiente.");

                item.UnitPrice = product.SalePrice;

                product.Stock -= item.Quantity;
                _productRepo.Update(product);

                _stockRepo.Add(new StockMovementModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Type = MovementTypeEnum.Saída,
                    TotalValue = item.Quantity * product.SalePrice,
                });
            }

            var rawTotal = sale.Items.Sum(i => i.Quantity * i.UnitPrice);
            var finalTotal = rawTotal;

            if (sale.ApplyCashback && sale.LoyalCustomer != null)
            {
                var customer = sale.LoyalCustomer;
                var maxUsable = Math.Min(customer.CashbackBalance, rawTotal);

                sale.CashbackUsed = maxUsable; // Atualizando o CashbackUsed

                finalTotal -= maxUsable;

                customer.CashbackBalance -= maxUsable;
                _customerRepo.Update(customer);
            }

            decimal cashbackPercentage = 0m;

            if (finalTotal < 50)
                cashbackPercentage = 0.01m;
            else if (finalTotal <= 100)
                cashbackPercentage = 0.015m;
            else
                cashbackPercentage = 0.02m;

            decimal cashbackEarned = finalTotal * cashbackPercentage;

            if (sale.LoyalCustomer != null)
            {
                sale.LoyalCustomer.CashbackBalance += cashbackEarned;
                _customerRepo.Update(sale.LoyalCustomer);
            }

        
            _saleRepo.Add(sale); 
            _context.SaveChanges();  

            return sale.Id;
        }



    }
}
