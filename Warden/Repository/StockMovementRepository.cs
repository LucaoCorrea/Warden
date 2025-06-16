using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models;

namespace Warden.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly AppDbContext _context;

        public StockMovementRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<StockMovementModel> GetAll()
        {
            return _context.StockMovement.OrderByDescending(m => m.CreatedAt).ToList();
        }

        public IEnumerable<StockMovementModel> GetAllWithProduct()
        {
            return _context.StockMovement
                .Include(m => m.Product)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }

        public void Add(StockMovementModel movement)
        {
            _context.StockMovement.Add(movement);
            _context.SaveChanges();
        }
    }
}
