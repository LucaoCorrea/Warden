using System.Collections.Generic;
using Warden.Models;

namespace Warden.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly List<StockMovementModel> _movements = new();

        public IEnumerable<StockMovementModel> GetAll() => _movements;

        public void Add(StockMovementModel movement)
        {
            movement.Id = _movements.Count > 0 ? _movements.Max(m => m.Id) + 1 : 1;
            movement.Date = DateTime.Now;
            _movements.Add(movement);
        }
    }
}
