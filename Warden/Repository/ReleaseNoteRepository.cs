using Warden.Data;
using Warden.Models;

namespace Warden.Repository
{
    public class ReleaseNoteRepository : IReleaseNoteRepository
    {
        private readonly AppDbContext _context;

        public ReleaseNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public ReleaseNoteModel GetLatest()
        {
            return _context.ReleaseNotes
                .OrderByDescending(r => r.ReleaseDate)
                .FirstOrDefault();
        }
    }

}
