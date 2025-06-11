using System.Collections.Generic;
using System.Linq;
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
            return _context.stockMovement.ToList();
        }

        public void Add(StockMovementModel movement)
        {
            movement.Date = DateTime.Now;
            _context.stockMovement.Add(movement);
            _context.SaveChanges();
        }
    }
}
