using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Warden.Data;
using Warden.Models;

namespace Warden.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SaleModel> GetAll()
        {
            return _context.Sales.Include(s => s.Items).ThenInclude(i => i.Product).ToList();
        }

        public SaleModel GetById(int id)
        {
            return _context.Sales.Include(s => s.Items).ThenInclude(i => i.Product)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Add(SaleModel sale)
        {
            _context.Sales.Add(sale);
            _context.SaveChanges();
        }
    }
}