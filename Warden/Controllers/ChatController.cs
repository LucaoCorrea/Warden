using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warden.Data;
using Warden.Models;

public class ChatController : Controller
{
    private readonly AppDbContext _context;

    public ChatController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _context.Chat
            .OrderByDescending(c => c.SentAt)
            .ToListAsync();

        return View(messages);
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult GetMessages()
    {
        var messages = _context.Chat
                        .OrderByDescending(m => m.SentAt)
                        .ToList();

        return PartialView("_MessagesPartial", messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string text, string userName)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(userName))
            return RedirectToAction("Index");

        var message = new ChatModel
        {
            Text = text,
            SentAt = DateTime.Now,
            UserName = userName  // Apenas armazenamos o nome do usuário
        };

        _context.Chat.Add(message);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
