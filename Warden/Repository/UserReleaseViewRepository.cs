using Warden.Data;
using Warden.Models;

namespace Warden.Repository
{
    public class UserReleaseViewRepository : IUserReleaseViewRepository
    {
        private readonly AppDbContext _context;

        public UserReleaseViewRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool HasUserViewed(string userName, int releaseNoteId)
        {
            return _context.UserReleaseViews
                .Any(x => x.UserName == userName && x.ReleaseNoteId == releaseNoteId);
        }

        public void MarkAsViewed(string userName, int releaseNoteId)
        {
            var view = new UserReleaseViewModel
            {
                UserName = "Warden Ltda;",
                ReleaseNoteId = releaseNoteId,
                ViewedAt = DateTime.Now
            };

            _context.UserReleaseViews.Add(view);
            _context.SaveChanges();
        }
    }

}
