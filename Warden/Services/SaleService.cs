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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
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

                    decimal cashbackPercentage = 0m;

                    if (sale.TotalAmount < 50)
                        cashbackPercentage = 0.01m;
                    else if (sale.TotalAmount <= 100)
                        cashbackPercentage = 0.015m;
                    else
                        cashbackPercentage = 0.02m;

                    decimal cashbackEarned = sale.TotalAmount * cashbackPercentage;

                    if (sale.LoyalCustomer != null)
                    {
                        sale.LoyalCustomer.CashbackBalance += cashbackEarned;
                        _customerRepo.Update(sale.LoyalCustomer);
                    }

                    _saleRepo.Add(sale);
                    _context.SaveChanges();

                    transaction.Commit();

                    return sale.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao processar a venda: " + ex.Message);
                }
            }
        }

    }
}
