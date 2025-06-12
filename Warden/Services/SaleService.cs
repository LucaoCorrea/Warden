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

        public int ProcessSale(SaleModel sale)
        {
            foreach (var item in sale.Items)
            {
                var product = _productRepo.GetById(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception($"Produto {item.ProductId} sem estoque suficiente.");

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

            _saleRepo.Add(sale);
            return sale.Id;
        }
    }
}
