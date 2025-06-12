using System.Collections.Generic;
using Warden.Models;

namespace Warden.Repositories
{
    public interface ISaleRepository
    {
        IEnumerable<SaleModel> GetAll();
        SaleModel GetById(int id);
        void Add(SaleModel sale);
    }
}