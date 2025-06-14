using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Enums;
using Warden.Models;
using Warden.Repositories;

namespace Warden.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStockMovementRepository _stockRepo;
        private readonly AppDbContext _context;

        public SaleService(ISaleRepository saleRepo, IProductRepository productRepo, IStockMovementRepository stockRepo, AppDbContext context)
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _context = context;
        }

        public IEnumerable<SaleModel> GetAll()
        {
            return _context.Sales
                .Include(s => s.Items)
                .ToList();
        }

        public SaleModel GetById(int id)
        {
            return _context.Sales
                .Include(s => s.Items)
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

            sale.TotalAmount = sale.Items.Sum(i => i.Quantity * i.UnitPrice);

            _saleRepo.Add(sale);
            return sale.Id;
        }

    }
}
