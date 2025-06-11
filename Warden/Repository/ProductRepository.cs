using System.Collections.Generic;
using System.Linq;
using Warden.Data;
using Warden.Models;

namespace Warden.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductModel> GetAll() => _context.Products.ToList();

        public ProductModel GetById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);

        public void Add(ProductModel product)
        {
            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(ProductModel product)
        {
            var existing = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.SKU = product.SKU;
                existing.Description = product.Description;
                existing.Category = product.Category;
                existing.Stock = product.Stock;
                existing.CostPrice = product.CostPrice;
                existing.SalePrice = product.SalePrice;
                existing.ImageUrl = product.ImageUrl;
                existing.Unit = product.Unit;

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
