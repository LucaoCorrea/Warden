using Microsoft.AspNetCore.Mvc;
using Warden.Data;
using Warden.Models;

namespace Warden.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var notifications = _context.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationModel notification)
        {
            if (notification == null) return BadRequest("Notificação inválida.");

            notification.CreatedAt = DateTime.Now;
            notification.IsRead = false;

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return Ok(notification);
        }

        [HttpPut("{id}")]
        public IActionResult MarkAsRead(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification == null) return NotFound("Notificação não encontrada.");

            notification.IsRead = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}
