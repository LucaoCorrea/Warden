using System.Collections.Generic;
using Warden.Models;

namespace Warden.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<ProductModel> GetAll();
        ProductModel GetById(int id);
        void Add(ProductModel product);
        void Update(ProductModel product);
        void Delete(int id);
    }
}
