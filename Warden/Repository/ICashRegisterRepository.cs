using System.Collections.Generic;
using Warden.Models;

namespace Warden.Repositories
{
    public interface ICashRegisterRepository
    {
        CashRegisterModel? GetOpenRegister();
        CashRegisterModel? GetById(int id);
        IEnumerable<CashRegisterModel> GetAll();
        void Add(CashRegisterModel register);
        void Update(CashRegisterModel register);
    }
}
