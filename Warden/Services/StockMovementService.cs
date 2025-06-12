using Warden.Enums;
using Warden.Models;
using Warden.Repositories;

namespace Warden.Services
{
    public class StockMovementService
    {
        private readonly IStockMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;

        public StockMovementService(IStockMovementRepository movementRepository, IProductRepository productRepository)
        {
            _movementRepository = movementRepository;
            _productRepository = productRepository;
        }

        public IEnumerable<StockMovementModel> GetAll()
        {
            return _movementRepository.GetAllWithProduct();
        }

        public void Add(StockMovementModel model)
        {
            var product = _productRepository.GetById(model.ProductId);
            if (product == null)
                throw new Exception("Produto não encontrado.");

            if (model.Type == MovementTypeEnum.Entrada)
            {
                product.Stock += model.Quantity;
                model.TotalValue = model.Quantity * product.CostPrice;
            }
            else if (model.Type == MovementTypeEnum.Saída)
            {
                if (product.Stock < model.Quantity)
                    throw new Exception("Estoque insuficiente para realizar a saída.");

                product.Stock -= model.Quantity;
                model.TotalValue = model.Quantity * product.SalePrice;
            }

            _movementRepository.Add(model);
            _productRepository.Update(product);
        }


    }
}
