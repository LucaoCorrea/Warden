using Warden.Data;
using Warden.Models;
using System.Collections.Generic;
using System.Linq;

namespace Warden.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public List<NotificationModel> GetUnreadNotifications()
        {
            return _context.Notifications.Where(n => !n.IsRead).ToList();
        }

        public NotificationModel GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void Update(NotificationModel notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }
    }
}
