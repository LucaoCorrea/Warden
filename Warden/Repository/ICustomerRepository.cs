using Warden.Models;

namespace Warden.Repository
{
    public interface ICustomerRepository
    {
        List<LoyalCustomerModel> GetAll();
        LoyalCustomerModel? GetById(int id);

        void Update(LoyalCustomerModel customer);
    }
}
