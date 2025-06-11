using System.Collections.Generic;
using Warden.Models;
using Warden.Repositories;

namespace Warden.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductModel> GetAll() => _productRepository.GetAll();

        public ProductModel GetById(int id) => _productRepository.GetById(id);

        public void Add(ProductModel product)
        {
            product.CreatedAt = DateTime.Now;
            _productRepository.Add(product);
        }

        public void Update(ProductModel product)
        {
            _productRepository.Update(product);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }
    }
}
