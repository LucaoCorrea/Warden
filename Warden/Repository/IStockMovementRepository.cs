﻿using System.Collections.Generic;
using Warden.Models;

namespace Warden.Repositories
{
    public interface IStockMovementRepository
    {
        IEnumerable<StockMovementModel> GetAll();
        IEnumerable<StockMovementModel> GetAllWithProduct(); 
        void Add(StockMovementModel movement);
    }
}
