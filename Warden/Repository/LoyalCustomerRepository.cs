using Warden.Data;
using Warden.Models;
using System;

namespace Warden.Repository
{
    public class LoyalCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public LoyalCustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<LoyalCustomerModel> GetAll()
        {
            return _context.LoyalCustomers.ToList();
        }

        public LoyalCustomerModel? GetById(int id)
        {
            return _context.LoyalCustomers.FirstOrDefault(c => c.Id == id);
        }

        public void Update(LoyalCustomerModel customer)
        {
            var existingCustomer = _context.LoyalCustomers.Find(customer.Id);
            if (existingCustomer == null)
                throw new Exception("Cliente não encontrado.");

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.CEP = customer.CEP;
            existingCustomer.Age = customer.Age;
            existingCustomer.CashbackBalance = customer.CashbackBalance;

            _context.SaveChanges();
        }
    }
}
