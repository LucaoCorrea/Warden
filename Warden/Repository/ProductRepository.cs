using System.Collections.Generic;
using System.Linq;
using Warden.Models;

namespace Warden.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<ProductModel> _products = new();

        public IEnumerable<ProductModel> GetAll() => _products;

        public ProductModel GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(ProductModel product)
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            product.CreatedAt = DateTime.Now;
            _products.Add(product);
        }

        public void Update(ProductModel product)
        {
            var existing = GetById(product.Id);
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
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
                _products.Remove(product);
        }
    }
}
