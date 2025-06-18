using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Warden.Data;
using Warden.Models;

namespace Warden.Controllers
{
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
                .Include(c => c.User)
                .OrderByDescending(c => c.SentAt)
                .ToListAsync();

            return View(messages);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Send(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return RedirectToAction("Index");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                return Unauthorized();

            var message = new ChatModel
            {
                Text = text,
                SentAt = DateTime.Now,
                User = user
            };

            _context.Chat.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
