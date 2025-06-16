using Warden.Models;

namespace Warden.Repository
{
    public interface IReleaseNoteRepository
    {
        ReleaseNoteModel GetLatest();
    }
}
