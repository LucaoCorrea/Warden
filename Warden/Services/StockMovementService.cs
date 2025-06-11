using System.Collections.Generic;
using Warden.Enums;
using Warden.Models;
using Warden.Repositories;

namespace Warden.Services
{
    public class StockMovementService
    {
        private readonly IStockMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;

        public StockMovementService(IStockMovementRepository movementRepo, IProductRepository productRepo)
        {
            _movementRepository = movementRepo;
            _productRepository = productRepo;
        }

        public IEnumerable<StockMovementModel> GetAll() => _movementRepository.GetAll();

        public void Add(StockMovementModel movement)
        {
            var product = _productRepository.GetById(movement.ProductId);
            if (product == null) return;

            if (movement.Type == MovementTypeEnum.Entry)
                product.Stock += movement.Quantity;
            else if (movement.Type == MovementTypeEnum.Exit)
                product.Stock -= movement.Quantity;

            _productRepository.Update(product);
            _movementRepository.Add(movement);
        }
    }
}
