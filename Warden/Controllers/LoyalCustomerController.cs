using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models;

public class LoyalCustomerController : Controller
{
    private readonly AppDbContext _context;

    public LoyalCustomerController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await _context.LoyalCustomers.ToListAsync();
        return View(customers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(LoyalCustomerModel model)
    {
        if (!ModelState.IsValid) return View(model);

        bool nameExists = await _context.LoyalCustomers.AnyAsync(c => c.Name == model.Name);
        bool emailExists = await _context.LoyalCustomers.AnyAsync(c => c.Email == model.Email);

        if (nameExists)
            ModelState.AddModelError("Name", "Já existe um cliente com este nome.");

        if (emailExists)
            ModelState.AddModelError("Email", "Já existe um cliente com este email.");

        if (!ModelState.IsValid)
            return View(model);

        _context.LoyalCustomers.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public IActionResult Export()
    {
        var customers = _context.LoyalCustomers.ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Warden Lovers");

        worksheet.Cell("A1").Value = "Nome";
        worksheet.Cell("B1").Value = "Email";
        worksheet.Cell("C1").Value = "CEP";
        worksheet.Cell("D1").Value = "Idade";
        worksheet.Cell("E1").Value = "Cashback";

        int row = 2;
        foreach (var c in customers)
        {
            worksheet.Cell(row, 1).Value = c.Name;
            worksheet.Cell(row, 2).Value = c.Email;
            worksheet.Cell(row, 3).Value = c.CEP;
            worksheet.Cell(row, 4).Value = c.Age;
            worksheet.Cell(row, 5).Value = c.CashbackBalance;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ClientesWarden.xlsx");
    }

}
