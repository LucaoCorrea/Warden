using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models;

namespace Warden.Repositories
{
    public class CashRegisterRepository : ICashRegisterRepository
    {
        private readonly AppDbContext _context;

        public CashRegisterRepository(AppDbContext context)
        {
            _context = context;
        }

        public CashRegisterModel? GetOpenRegister()
        {
            return _context.CashRegisters
                .Include(r => r.Movements)
                .FirstOrDefault(r => r.ClosedAt == null);
        }

        public CashRegisterModel? GetById(int id)
        {
            return _context.CashRegisters
                .Include(r => r.Movements)
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<CashRegisterModel> GetAll()
        {
            return _context.CashRegisters.Include(r => r.Movements).ToList();
        }

        public void Add(CashRegisterModel register)
        {
            _context.CashRegisters.Add(register);
            _context.SaveChanges();
        }

        public void Update(CashRegisterModel register)
        {
            _context.CashRegisters.Update(register);
            _context.SaveChanges();
        }
    }
}
