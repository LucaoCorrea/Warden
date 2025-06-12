using Warden.Data;
using Warden.Models;

namespace Warden.Services
{
    public class CashRegisterService
    {
        private readonly AppDbContext _context;

        public CashRegisterService(AppDbContext context)
        {
            _context = context;
        }

        public CashRegisterModel? GetOpenRegister()
        {
            return _context.CashRegisters
                           .Where(c => c.ClosedAt == null)
                           .OrderByDescending(c => c.OpenedAt)
                           .FirstOrDefault();
        }

        private CashRegisterModel? GetLastClosedRegister()
        {
            return _context.CashRegisters
                .Where(c => c.ClosedAt != null)
                .OrderByDescending(c => c.ClosedAt)
                .FirstOrDefault();
        }

        public CashRegisterModel OpenRegister()
        {
            var alreadyOpen = GetOpenRegister();
            if (alreadyOpen != null)
                throw new InvalidOperationException("Já existe um caixa aberto.");

            var lastClosed = GetLastClosedRegister();
            decimal newInitialAmount = lastClosed?.FinalAmount ?? 0m;

            var newRegister = new CashRegisterModel
            {
                OpenedAt = DateTime.Now,
                InitialAmount = newInitialAmount,
                OpenedBy = "Teste" // Isto é uma gambiarra para testar o user, nao esta passando o user da sessao
            };

            _context.CashRegisters.Add(newRegister);
            _context.SaveChanges();

            return newRegister;
        }


        public bool CloseRegister(decimal closingAmount)
        {
            var open = GetOpenRegister();
            if (open == null)
                return false;

            var openedAt = open.OpenedAt;

            decimal movementsTotal = _context.stockMovement
                .Where(m => m.CreatedAt >= openedAt)
                .Sum(m => m.TotalValue);

            // valor final = valor do caixa + vendas desde a abertura
            decimal finalValue = open.InitialAmount + movementsTotal;

            open.ClosedAt = DateTime.Now;
            open.FinalAmount = finalValue;
            open.ClosedBy = "Teste";
            open.Difference = finalValue - open.InitialAmount;

            _context.CashRegisters.Update(open);
            _context.SaveChanges();

            return true;
        }

        public List<CashRegisterModel> GetAll()
        {
            return _context.CashRegisters
                           .OrderByDescending(c => c.OpenedAt)
                           .ToList();
        }
    }
}
